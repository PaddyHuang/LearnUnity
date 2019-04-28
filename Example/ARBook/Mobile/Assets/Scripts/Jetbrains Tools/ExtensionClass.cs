using UnityEngine;
using JetBrains.Annotations;

public static class ExtensionClass
{
    /// <summary> 
    /// 朝向目标 </summary>
    public static void LookAtTargetAroundY(this Transform source, Transform target)
    {
        //计算物体在朝向某个向量后的正前方
        Vector3 forwardDir = source.position - target.position;

        //计算朝向这个正前方时的物体四元数值
        Quaternion lookAtRot = Quaternion.LookRotation(forwardDir);

        //把四元数值转换成角度
        float resultEuler = lookAtRot.eulerAngles.y;

        source.eulerAngles = new Vector3(0, resultEuler, -90);
    }

    /// <summary> 
    /// 朝向目标 </summary>
    public static void LookAtTargetAroundZ(this Transform source, Transform target)
    {
        //计算物体在朝向某个向量后的正前方
        Vector3 forwardDir = source.position - target.position;

        //计算朝向这个正前方时的物体四元数值
        Quaternion lookAtRot = Quaternion.LookRotation(forwardDir);

        //把四元数值转换成角度
        float resultEuler = lookAtRot.eulerAngles.z;

        source.eulerAngles = new Vector3(0, 0, resultEuler);
    }

    public static float Angle360WithX(this Vector3 fromV3, Vector3 toV3)
    {
        float ro = Vector3.Angle(fromV3, toV3);

        Vector3 cross = Vector3.Cross(fromV3, toV3);

        if (cross.y < 0)
            ro = 360 - ro;

        return ro;
    }

    public static float AngleWithY(this Vector3 fromV3, Vector3 toV3)
    {
        float ro = Vector3.Angle(fromV3, toV3);

        Vector3 cross = Vector3.Cross(fromV3, toV3);

        if (cross.x < 0)
            return -ro;
        //Debug.Log(cross);

        return ro;
    }

    /// <summary> 
    /// 是否朝向某物体,true = 面对,false = 背对 </summary>
    public static bool FaceDirection(this Transform source, Transform target)
    {
        return Vector3.Dot(target.position - source.position, source.forward) > 0;
    }

    /// <summary> 
    /// 是否朝向某物体,true = 面对,false = 背对 </summary>
    public static bool FaceDirection(this Transform source, Vector3 targetPos)
    {
        return Vector3.Dot(targetPos - source.position, source.forward) > 0;
    }

    [SourceTemplate]
    public static void IsNull(this Object obj)
    {
        /*$if (!ReferenceEquals(obj, null))
         {
             $END$
         }*/
    }
}

/*$ var $entity$Id = entity.GetId();
DoSomething($entity$Id); */