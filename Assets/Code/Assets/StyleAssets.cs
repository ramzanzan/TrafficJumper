using UnityEngine;

public abstract class StyleAssets : ScriptableObject
{
    public CommonAssets CommonAssets;
    
    public Texture2D[] C1_T;
    public Texture2D[] C2_T;
    public Texture2D[] C3_T;
    public Texture2D[] P_T;
    public GameObject Car1, Car2, Car3;
    public GameObject Police;
    public GameObject Military, Trailer;
    public GameObject Projectile;
    public GameObject Copter;
    public GameObject Missile;
    public GameObject FirstPuddle;
    public GameObject TransitPuddle;
    public Texture2D[] TransitPuddle_T;
    public Texture2D LastPuddle_T;
    public GameObject Wall;            
    public GameObject Road;            
    public GameObject Meteor;
    public GameObject MeteorProjector;

    public abstract void InitializeStyle();

    public virtual void StylizeCar1(GameObject go){}
    public virtual void StylizeCar2(GameObject go){}
    public virtual void StylizeCar3(GameObject go){}
    public virtual void StylizePolice(GameObject go){}
    public virtual void StylizeMilitary(GameObject go){}
    public virtual void StylizeTrailer(GameObject go){}
    public virtual void StylizeCopter(GameObject go){}
    public virtual void StylizeFirstPuddle(GameObject go){}
    public virtual void StylizeTransitPuddle(GameObject go){}
    public virtual void StylizeLastPuddle(GameObject go){}
    public virtual void StylizeMeteor(GameObject meteor, GameObject projector){}
    public virtual void StylizeWall(GameObject go){}
    public virtual void StylizeRoad(GameObject go){}
    
}
