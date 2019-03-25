using System;
using System.Collections;
using System.Collections.Generic;
using Code.Gameplay.Tasks;
using UnityEngine;

public class GameplayController : MonoBehaviour
{
	private static GameplayController _instance;
	
	public static void InitializeInstance()
	{
		_instance.Initialize();
	}
	
	public static GameplayController GetInstance()
	{
		if(_instance==null) throw new Exception("Сlasses initialization order have violated");
		if(!_instance._isInitialized) throw new Exception("Class wasn't initialized");
		return _instance;
	}
	
	private void Start () {
		if(_instance!=null) throw new Exception("Second singleton");
		_instance = this;
	}

	private bool _isInitialized;
	private BuildController _buildCtrl;
	private TaskController _taskCtrl;
	private PursuitController _pursuitCtrl;
	private AvatarController _avatarCtrl;
	private PlayerAvatar _avatar;
	private IDGenerator _idGen;
	private IDictionary<int, GameObject> _stuff;
	private MultiGeneratorProvider _mainProvider;
	private MultiGenerator _mainGenerator;
	private Level _level;
	private DifficultController _difficultCtrl;
	private PlaySettings _playSets;
	private int _idOfLast;
	private CarDescriptor _firstCarDesc;
	private bool _gameOn;
	private bool _pause;
	private float _timeScale;
	private Vector2 _firstCarPos;
	private PlayStatistics _playStats;
	private const float DisassembleTime = 10;
	private float _cdTimer;
	private float _traveledDistance;
	private int _score;
	private GlobalStatistics _globStats;
	
	public State CurrentState { get; private set; }
	public Level CurrentLevel => _level;
	public event Action StateChanged;
	public Action<int> ScoreChanged;
	public Action<float> DistanceChanged;
	public Action Died;
	
	public enum State
	{
		Idle,
		Play,
		Dead,
		Pause
	}

	public float TraveledDistance
	{
		get { return _traveledDistance;}
		private set
		{
			if(_traveledDistance!=value)
				DistanceChanged?.Invoke(value);
			_traveledDistance = value;
			
		}
	}
	public int Score
	{
		get { return _score; }
		private set
		{
			if(_score!=value)
				ScoreChanged?.Invoke(value);
			_score = value;
		}
	}

	
	private void Initialize()
	{
		CurrentState = State.Idle;
		_avatarCtrl = AvatarController.GetInstance();
		_avatar = _avatarCtrl.Avatar;
		_avatar.OnSuccsefulEating += () => { Score += 1; };
		_avatar.OnDeath += DeathHandler;
		_buildCtrl = BuildController.GetInstance();
		_buildCtrl.SetMeteorController();
		_taskCtrl = TaskController.GetInstance();
		_taskCtrl.Avatar = _avatar;
		_taskCtrl.TaskCompleted += (ITask x) => Debug.Log(x);
		DuringDistanceTask.Avatar = _avatar.transform;
		_pursuitCtrl = PursuitController.GetInstance();
		_pursuitCtrl.AvatarTf = _avatar.transform;
		_pursuitCtrl.Catched += CatchedHandler;
		_idGen = new IDGenerator();
		_stuff = new SortedDictionary<int, GameObject>();
		_mainProvider = new MultiGeneratorProvider(_stuff, _pursuitCtrl, _idGen);
		_mainProvider.SetIDGenerator(_idGen);
		_firstCarPos = new Vector2(0,1);
		_firstCarDesc = new CarDescriptor(_firstCarPos,0,1);
		_playStats = new PlayStatistics();
		_globStats = GlobalStatistics.GetInstance();
//        
		Vehicle.Avatar = _avatar;
		ProjectileGun.SetAvatar(_avatar);
		_isInitialized = true;
	}
	
	public void SetLevel(Level level)
	{
		if(_gameOn)
			EndLevel();
		_level = level;
		_taskCtrl.SetLevel(level);
		
		_difficultCtrl = level.DifficultCtrl;
		_playSets = level.PlaySettings;
		_mainProvider.SetGenerator(level.MultiGenerator);
		
		_avatarCtrl.Avatar.Init(_playSets.MaxJumpDistance,_playSets.MinJumpDistance,
			_playSets.HorizontalVelocity,_playSets.PowerTime,_playSets.MaxJumpHeight);
        _avatarCtrl.Pointer.Init(_playSets.MaxJumpDistance,.4f,_playSets.MinJumpDistance);
		
		Vehicle.Road = _playSets.Road;
		
		_firstCarDesc.Velocity = _playSets.NormalCarSpeed;
		_pursuitCtrl.MinVelocity = _playSets.MinPursuitVelocity;
		float t1 = (_playSets.MaxJumpDistance - _playSets.MinCarGap)/ ( 2 * _playSets.MaxJumpDistance) * _playSets.PowerTime;
		float t2 = (_playSets.MaxJumpDistance - _playSets.MinCarGap) / _playSets.HorizontalVelocity;
		_pursuitCtrl.MaxVelocity = _playSets.MaxCarSpeed*t1/(t1+t2) + (_playSets.MaxCarSpeed+ _playSets.HorizontalVelocity)*t2/(t1+t2);
		_pursuitCtrl.SetPursuitBoundsFactors(.5f,.99f);	//todo something?
	}

	public void PrepareLevel()
	{
		_stuff.Clear();
		_buildCtrl.RecallAll();	
		_idGen.Reset();
		_firstCarDesc.Position = _firstCarPos;
	}

	private void InstallAvatar()
	{
		_avatar.Rezet();
		_avatar.transform.position = new Vector3(0,.83f,1.5f);
	}
	
	public void StartLevel()
	{
		InstallAvatar(); //todo в подготовку уровня
		_taskCtrl.StartLevel();
		Score = 0;
		TraveledDistance = 0;
		_idOfLast = _firstCarDesc.GetID();
		Construct(_firstCarDesc);
		_mainProvider.Reset(_firstCarDesc);
		_pursuitCtrl.LevelStarted();
		_gameOn = true;
	}

	public void EndLevel()
	{
		_gameOn = false;
		_avatar.BreakDown();
		_pursuitCtrl.LevelEnded();
		_globStats.Score += Score;
	}

	public void Unpause()
	{
		if (!_pause) return;
		Time.timeScale = _timeScale;
		_pause = false;
	}

	public void Pause()
	{
		if (_pause) return;
		_timeScale = Time.timeScale;
		Time.timeScale = 0;
		_pause = true;
	}

	private void DeathHandler()
	{
		//todo
		Died?.Invoke();
		EndLevel();
	}

	private void CatchedHandler()
	{
		_avatar.Die();
	}

	private void Construct(IDescriptorWithID desc)
	{
        GameObject go;
		go = _buildCtrl.Build(desc);
        
		if(go==null) return;
        _stuff.Add(desc.GetID(),go);
		
        if (go.CompareTag("Car"))
            _idOfLast = desc.GetID();
	}

	private void Update ()
	{
		if(!_gameOn) return;
		
		_cdTimer -= Time.deltaTime;
		if (_cdTimer < 0)
		{
			_buildCtrl.DisassembleAllBelow(_pursuitCtrl.ScreenBottom());
			_cdTimer = DisassembleTime;
		}
			
        if (_mainProvider.HasMore())
        {
            if (_mainProvider.IsReadyForMore())
            {
                while (_mainProvider.HasNext())
                {
                    Construct(_mainProvider.Next());
                }
            }
        }
        else
        {
            var last = _stuff[_idOfLast].GetComponent<Vehicle>();
            var pos = new Vector2(last.transform.position.x,last.transform.position.z);
            var scd = new CarDescriptor(pos,last.Velocity.z,last.Size);
            scd.SetID(_idOfLast);
            _mainProvider.Reset(scd);
        }
		
		TraveledDistance = _avatar.transform.position.z;
	}
}
