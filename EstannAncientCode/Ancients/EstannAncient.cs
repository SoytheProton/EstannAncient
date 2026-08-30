using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using EstannAncient.EstannAncientCode.Relics;
using Godot;
using MegaCrit.Sts2.Core.Models;

namespace EstannAncient.EstannAncientCode.Ancients;

public class EstannAncient : CustomAncientModel
{
    protected override OptionPools MakeOptionPools =>

        new(
            MakePool(
                AncientOption<NameOne>(), 
                AncientOption<NameTwo>()),
            MakePool(
                AncientOption<NameEight>()
            ),
            MakePool(
                AncientOption<NameFive>(),
                AncientOption<NameSeven>()
            ));

    public override Color ButtonColor => new(0.05f, 0.05f, 0.15f, 0.8f);

    public override Color DialogueColor => new("161430");

    public override bool IsValidForAct(ActModel act)
    {
        return act.ActNumber() == 2;
    }
}