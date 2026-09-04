using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using EstannAncient.EstannAncientCode.Relics;
using Godot;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Models.Relics;

namespace EstannAncient.EstannAncientCode.Ancients;

public class EstannAncient : CustomAncientModel
{
    private static Dictionary<ModelId, (AncientOption,AncientOption)> BaseCharacterRelics => new()
        {
            {
                ModelDb.Character<Ironclad>().Id,
                (AncientOption<Anchor>(), AncientOption<Anchor>())
            },
            {
                ModelDb.Character<Silent>().Id,
                (AncientOption<Anchor>(), AncientOption<Anchor>())
            },
            {
                ModelDb.Character<Regent>().Id,
                (AncientOption<Anchor>(), AncientOption<Anchor>())
            },
            {
                ModelDb.Character<Necrobinder>().Id,
                (AncientOption<Anchor>(), AncientOption<Anchor>())
            },
            {
                ModelDb.Character<Defect>().Id,
                (AncientOption<Anchor>(), AncientOption<Anchor>())
            }
        };

    private static Dictionary<ModelId, (AncientOption,AncientOption)>? _characterRelics;
    
    protected override OptionPools MakeOptionPools =>

        new(
            MakePool(
                AncientOption<NameOne>(), 
                AncientOption<NameTwo>(),
                AncientOption<NameSix>()),
            MakeCharacterPool(),
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

    private WeightedList<AncientOption> MakeCharacterPool()
    {
        // will need to change BaseCharacterRelic to a more dynamic system to allow for custom relics.
        // The Anchors mentioned here are meant to be the generic relics.
        
        var list = new WeightedList<AncientOption> { AncientOption<NameEight>() };

        /*if (Owner == null || IsCanonical)
        {
            foreach (var tuple in BaseCharacterRelics.Select(kv => kv.Value))
            {
                list.Add(tuple.Item1);
                list.Add(tuple.Item2);
            }
            list.Add(AncientOption<Anchor>());
            list.Add(AncientOption<Anchor>());
        }
        else
        {
            if (BaseCharacterRelics.TryGetValue(Owner.Character.Id, out var tuple))
            {
                list.Add(tuple.Item1);
                list.Add(tuple.Item2);
            }
            else
            {
                list.Add(AncientOption<Anchor>());
                list.Add(AncientOption<Anchor>());
            }
        }*/
        
        return list;
    }
}