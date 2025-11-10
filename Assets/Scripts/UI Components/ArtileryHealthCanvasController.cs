using UnityEngine;

public class ArtileryHealthCanvasController : AllyHealthCanvasController
{
    protected override void UpdateText()
    {
        maxhealthText.text = attachedTower.MaxHealth+"";
        attackDamageText.text = attachedTower.AttackDamage + "";
    }
}
