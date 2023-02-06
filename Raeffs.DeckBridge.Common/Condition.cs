namespace Raeffs.DeckBridge.Common;

/// <summary>
/// https://www.cardmarket.com/en/Help/CardCondition
/// 
/// European -> American
/// Mint -> Mint
/// Near Mint -> Near Mint
/// Excellent -> Lightly Played
/// Good -> Moderately Played
/// Light Played -> Played
/// Played -> Heavily Played
/// Poor -> Damaged
/// </summary>
public enum Condition
{
    Unknown,
    Mint,
    NearMint,
    Excellent,
    Good,
    LightPlayed,
    Played,
    Poor
}
