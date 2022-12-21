using Content.Server.Polymorph.Systems;
using Content.Shared.Audio;
using Content.Shared.Chemistry.Reagent;
using Content.Shared.Polymorph;
using Content.Shared.Popups;
using JetBrains.Annotations;
using Robust.Shared.Audio;
using Robust.Shared.Player;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Server.Chemistry.ReagentEffects;

public sealed class Polymorph : ReagentEffect
{
    [DataField("polymorphId", required: true, customTypeSerializer: typeof(PrototypeIdSerializer<PolymorphPrototype>))]
    [ViewVariables(VVAccess.ReadWrite)]
    public readonly string PolymorphId = default!;

    [DataField("polymorphSound")]
    [ViewVariables(VVAccess.ReadWrite)]
    public SoundSpecifier? PolymorphSound;

    [DataField("polymorphMessage")]
    [ViewVariables(VVAccess.ReadWrite)]
    public string? PolymorphMessage;

    public override void Effect(ReagentEffectArgs args)
    {
        EntityUid? polyUid = EntitySystem.Get<PolymorphableSystem>().PolymorphEntity(args.SolutionEntity, PolymorphId);

        if (PolymorphSound != null && polyUid != null)
            SoundSystem.Play(PolymorphSound.GetSound(), Filter.Pvs(polyUid.Value), polyUid.Value, AudioHelpers.WithVariation(0.2f));

        if (PolymorphMessage != null && polyUid != null)
            EntitySystem.Get<SharedPopupSystem>().PopupEntity(Loc.GetString(PolymorphMessage), polyUid.Value, polyUid.Value, Shared.Popups.PopupType.Large);
    }
}
