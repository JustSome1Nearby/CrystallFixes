using Content.Shared.Electrocution;
using Robust.Shared.Prototypes;

namespace Content.Shared.EntityEffects.Effects;

public sealed partial class Electrocute : EntityEffect
{
    [DataField] public float ElectrocuteTime = 2; //CP14 - float

    [DataField] public int ElectrocuteDamageScale = 5;

    /// <remarks>
    ///     true - refresh electrocute time,  false - accumulate electrocute time
    /// </remarks>
    [DataField] public bool Refresh = true;

    protected override string? ReagentEffectGuidebookText(IPrototypeManager prototype, IEntitySystemManager entSys)
        => Loc.GetString("reagent-effect-guidebook-electrocute", ("chance", Probability), ("time", ElectrocuteTime));

    public override bool ShouldLog => true;

    public override void Effect(EntityEffectBaseArgs args)
    {
        if (args is EntityEffectReagentArgs reagentArgs)
        {
            reagentArgs.EntityManager.System<SharedElectrocutionSystem>().TryDoElectrocution(reagentArgs.TargetEntity, null,
                Math.Max((reagentArgs.Quantity * ElectrocuteDamageScale).Int(), 1), TimeSpan.FromSeconds(ElectrocuteTime), Refresh, ignoreInsulation: true);

            if (reagentArgs.Reagent != null)
                reagentArgs.Source?.RemoveReagent(reagentArgs.Reagent.ID, reagentArgs.Quantity);
        } else
        {
            args.EntityManager.System<SharedElectrocutionSystem>().TryDoElectrocution(args.TargetEntity, null,
                Math.Max(ElectrocuteDamageScale, 1), TimeSpan.FromSeconds(ElectrocuteTime), Refresh, ignoreInsulation: true);
        }
    }
}
