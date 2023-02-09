namespace Raeffs.DeckBridge.Common;

/// <summary>
/// https://www.cardmarket.com/en/Help/CardCondition
/// </summary>
public enum Condition
{
    Unknown,

    /// <summary>
    /// American equivalent: Mint
    /// </summary>
    Mint,

    /// <summary>
    /// American equivalent: Near Mint
    /// </summary>
    NearMint,

    /// <summary>
    /// American equivalent: Slightly Played or Lightly Played
    /// </summary>
    Excellent,

    /// <summary>
    /// American equivalent: Moderately Played
    /// </summary>
    Good,

    /// <summary>
    /// American equivalent: Played
    /// </summary>
    LightPlayed,

    /// <summary>
    /// American equivalent: Heavily Played
    /// </summary>
    Played,

    /// <summary>
    /// American equivalent: Poor or Damaged
    /// </summary>
    Poor
}
