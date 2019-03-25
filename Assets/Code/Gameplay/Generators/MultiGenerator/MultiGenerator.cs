using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MultiGenerator
{
	private int _minCount;
	private int _maxCount;
	private RandomItem<IMiniGenerator> _gensDistr;
	private IMiniGenerator _blockGen;
	private RandomItem<IBlockModifier> _modsDistr;
	private IBlockModifier _blockModifier;
	private float _modProb;
	private int _maxModCount;

	public bool UseInitGen;
	public BlockGenerator InitGenerator;
	public PlaySettings PS;
	public int Count { get; private set; }
	public Func<IDictionary<int, GameObject>, LinkedList<IDescriptorWithID>, float, bool> ReadinessTestHandler { get; private set; }
	public IDGenerator IDGen;
	public LinkedList<IDescriptorWithID> DescriptorList { get; }

	public MultiGenerator(int minCount, int maxCount, RandomItem<IMiniGenerator> miniGens,
		float modProbability=0, int maxModCount=0, RandomItem<IBlockModifier> blockMods=null)
	{
		if(miniGens==null || miniGens.Length==0) throw new ArgumentException();
		if(blockMods!=null && blockMods.Length==0) throw new ArgumentException();
		_minCount = minCount;
		_maxCount = maxCount;
		_gensDistr = miniGens;
		_modsDistr = blockMods;
		_modProb = modProbability;
		_maxModCount = maxModCount;
		DescriptorList = new LinkedList<IDescriptorWithID>();
	}
	
	public MultiGenerator(int minCount, int maxCount, IMiniGenerator miniGen) :
		this(minCount, maxCount, new RandomItem<IMiniGenerator>(new []{miniGen}))
	{}
	
	public void Generate()
	{	
		_blockGen.GenerateBlock(DescriptorList);
		
		if (_modsDistr!=null && _modProb>0 && _blockGen.WasBlockModifable)
		{
			if (_modProb - Random.Range(0, 1f) >= 0)
			{
				int i = Random.Range(1, _maxModCount + 1);
				for (; i > 0; --i)
				{			
					_blockModifier = _modsDistr.Next();
					_blockModifier.Modify(DescriptorList);
				}
			}
		}
		
		if(IDGen!=null)
			foreach (var e in DescriptorList)
				e.SetID(IDGen.NextID());
			
		--Count;
	}

	private void ResetBlockGen()
	{
		_blockGen = _gensDistr.Next();
		_blockGen.Reset();
		ReadinessTestHandler = _blockGen.ReadinessTestHandler;
	}
	
	/// <summary>
	/// Side effect: Resetting ReadinessTestHandler
	/// </summary>
	/// <returns></returns>
	public bool HasMore()
	{
		if (Count > 0 && !_blockGen.HasMore)
			ResetBlockGen();
		return Count > 0 || _blockGen.HasMore;
	}

	public void Reset()
	{
		if (UseInitGen && InitGenerator!=null)
		{
			_blockGen = InitGenerator;
			UseInitGen = false;
		}
		else
			_blockGen = _gensDistr.Next() ;
		_blockGen.Reset();
		ReadinessTestHandler = _blockGen.ReadinessTestHandler;
		Count = Random.Range(_minCount, _maxCount + 1);
	}
}
