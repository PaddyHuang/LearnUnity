/// <summary> 
/// 创建者接口 </summary>
public interface ICreatorHandler<T>
{
    /// <summary>
    /// 创建各种组件
    /// </summary>
    /// <param name="od">派生自ObjectData 的数据类型</param>
    /// <param name="types">所选择的类型</param>
    void CreatComponent(ObjectData od, T types);
}

/// <summary> 
/// 创建练习接口 </summary>
public interface IPracticeHandler
{
    /// <summary>
    /// 创建练习方法
    /// </summary>
    /// <param name="od">派生自ObjectData 的数据类型</param>
    void CreatPractice(ObjectData od);
}