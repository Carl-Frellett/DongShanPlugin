namespace RueI.Displays;


/// <summary>
/// Provides a means for describing multiple <see cref="RoleType"/>s.
/// </summary>
/// <remarks>
/// The purpose of the <see cref="Roles"/> enum is to enable roles to be treated like a <see cref="FlagsAttribute"/> enum. Normally,
/// <see cref="RoleType"/> cannot be treated like bit flags, so this acts as a fast and convenient way to do so.
/// </remarks>
[Flags]
public enum Roles
{
    /// <summary>
    /// Gets the SCP-173 role id.
    /// </summary>
    Scp173 = 1 << RoleType.Scp173,

    /// <summary>
    /// Gets the Class-D role id.
    /// </summary>
    ClassD = 1 << RoleType.ClassD,

    /// <summary>
    /// Gets the Specatator role id.
    /// </summary>
    Spectator = 1 << RoleType.Spectator,

    /// <summary>
    /// Gets the SCP-106 role id.
    /// </summary>
    Scp106 = 1 << RoleType.Scp106,

    /// <summary>
    /// Gets the NTF Specialist role id.
    /// </summary>
    NtfSpecialist = 1 << RoleType.NtfScientist,

    /// <summary>
    /// Gets the SCP-049 role id.
    /// </summary>
    Scp049 = 1 << RoleType.Scp049,

    /// <summary>
    /// Gets the Scientist role id.
    /// </summary>
    Scientist = 1 << RoleType.Scientist,

    /// <summary>
    /// Gets the SCP-079 role id.
    /// </summary>
    Scp079 = 1 << RoleType.Scp079,

    /// <summary>
    /// Gets the Chaos Insurgency role id.
    /// </summary>
    ChaosInsurgency = 1 << RoleType.ChaosInsurgency,

    /// <summary>
    /// Gets the SCP-096 role id.
    /// </summary>
    Scp096 = 1 << RoleType.Scp096,

    /// <summary>
    /// Gets the SCP-049-2 role id.
    /// </summary>
    Scp0492 = 1 << RoleType.Scp0492,

    /// <summary>
    /// Gets the NTF Sergeant role id.
    /// </summary>
    NtfSergeant = 1 << RoleType.NtfLieutenant,

    /// <summary>
    /// Gets the NTF Captain role id.
    /// </summary>
    NtfCaptain = 1 << RoleType.NtfCommander,

    /// <summary>
    /// Gets the NTF Private role id.
    /// </summary>
    NtfPrivate = 1 << RoleType.NtfCadet,

    /// <summary>
    /// Gets the Tutorial role id.
    /// </summary>
    Tutorial = 1 << RoleType.Tutorial,

    /// <summary>
    /// Gets the Facility Guard role id.
    /// </summary>
    FacilityGuard = 1 << RoleType.FacilityGuard,

    /// <summary>
    /// Gets the SCP-93989 role id.
    /// </summary>
    Scp93989 = 1 << RoleType.Scp93989,

    /// <summary>
    /// Gets the SCP-93989 role id.
    /// </summary>
    Scp93953 = 1 << RoleType.Scp93953,

    /// <summary>
    /// Gets all of the NTF role ids, including Facility Guards.
    /// </summary>
    NtfRoles = NtfPrivate | NtfSergeant | NtfSpecialist | NtfCaptain | FacilityGuard,

    /// <summary>
    /// Gets all of the military role ids.
    /// </summary>
    MilitaryRoles = NtfRoles | ChaosInsurgency | Tutorial,

    /// <summary>
    /// Gets all of the civilian role ids.
    /// </summary>
    CivilianRoles = ClassD | Scientist,

    /// <summary>
    /// Gets all of the human role ids.
    /// </summary>
    HumanRoles = MilitaryRoles | CivilianRoles,

    /// <summary>
    /// Gets all of the SCP role ids, excluding SCP-049-2.
    /// </summary>
    ScpsNo0492 = Scp173 | Scp106 | Scp049 | Scp079 | Scp096 | Scp93989 | Scp93953,

    /// <summary>
    /// Gets all of the SCP role ids, including SCP-049-2.
    /// </summary>
    Scps = ScpsNo0492 | Scp0492,

    /// <summary>
    /// Gets all of the role ids for roles considered to be alive.
    /// </summary>
    Alive = Scps | HumanRoles,

    /// <summary>
    /// Gets all of the role ids for roles considered to be dead.
    /// </summary>
    Dead = Spectator,

    /// <summary>
    /// Gets all role ids.
    /// </summary>
    All = Alive | Dead,
}