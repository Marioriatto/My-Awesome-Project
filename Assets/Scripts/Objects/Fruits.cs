using UnityEngine;
using System;
public class Fruits : Item
{
    public override void Pick()
    {
        base.Pick();
        Consume();
        return;
    }
    public override void OnConsume()
    {
        base.OnConsume();
    }
}
