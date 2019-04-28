public class UIMenuCard : Card
{
    public bool Locked;

    public bool EnterScene()
    {
        return Locked;
    }

    //[HideInInspector] public int id = 0;

    //public int CompareTo(object obj)
    //{
    //    Card otherCard = obj as Card;
    //    if (otherCard != null)
    //    {
    //        return id.CompareTo(otherCard.id);
    //    }
    //    else
    //    {
    //        return 0;
    //    }
    //}
}