using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;

namespace EstannAncient.EstannAncientCode.Ancients;


public class EstannAncient : CustomAncientModel
{
    protected override OptionPools MakeOptionPools =>

        new(
            MakePool(
                AncientOption<Anchor>(),
                AncientOption<Anchor>(),
                AncientOption<Anchor>()
            ),
            MakePool(
                AncientOption<Anchor>(3),
                AncientOption<Anchor>(3),
                AncientOption<Anchor>(3),
                AncientOption<Anchor>(2)
            ),
            MakePool(
                AncientOption<Anchor>(3),
                AncientOption<Anchor>(2),
                AncientOption<Anchor>(5)
            ));

    public override Color ButtonColor => new(0.05f, 0.05f, 0.15f, 0.8f);

    public override Color DialogueColor => new("161430");

    public override bool IsValidForAct(ActModel act)
    {
        return act.ActNumber() == 2;
    }
}