/// Small value types shared by GameState and its callers.

public enum Stat { None, Hp, Mind, Ward }

/// The five daily needs. Safety is met by having no CHECK open; the rest are chores.
public enum Need { Food, Water, Warmth, Social, Safety }

public enum DayPhase { Day, Night }

public enum LocationStatus { Safe, Anomaly }
