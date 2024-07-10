using System;
using System.Text.RegularExpressions;
using MapChanger;
using RandomizerCore.Logic;
using UnityEngine;

namespace RandoMapMod.Localization
{
    public static class Localization
    {
        /// <summary>
        /// Localize text
        /// </summary>
        public static string L(this string text)
        {
            return RandomizerMod.Localization.Localize(text);
        }

        /// <summary>
        /// Localize and clean text
        /// </summary>
        public static string LC(this string text)
        {
            return text.L().ToCleanName();
        }
        
        /// <summary>
        /// Localize and clean text in transition format A[B]
        /// </summary>
        public static string LT(this string name)
        {
            var match = Regex.Match(name, @"^(\w+)\[(\w+)\]$");

            if (match.Groups.Count == 3)
            {
                return $"{match.Groups[1].Value.LC()}[{match.Groups[2].Value.LC()}]";
            }

            return name.LC() ;
        }

        /// <summary>
        /// Localize text in logic
        /// </summary>
        public static string LL(this string name)
        {
            string pattern = @"[\w-']+";
            return Regex.Replace(name, pattern, new MatchEvaluator(ReplaceName));
        }

        private static string ReplaceName(Match match)
        {
#if DEBUG
            switch (match.Value)
            {
                case "before":
                case "after":
                case "Uumuu_Arena":
                case "COMBAT":
                case "Lit_Abyss_Lighthouse":
                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                case "6":
                case "7":
                case "8":
                    break;
                default:
                    if (match.Value != match.Value.L()) break;
                    if (match.Value.Contains("Floor")) break;
                    if (match.Value.Contains("Wall")) break;
                    if (match.Value.Contains("Defeated")) break;
                    if (match.Value.Contains("Opened")) break;
                    if (match.Value.Contains("Collector's_Map")) break;
                    if (match.Value.Contains("Lever")) break;
                    if (match.Value.Contains("Defeated")) break;
                    if (match.Value.Contains("HITS")) break;
                    if (match.Value.Contains("Geo")) break;
                    if (match.Value.Contains("Stag")) break;
                    if (match.Value.Contains("Hot_Spring")) break;
                    if (match.Value.Contains("Lumafly")) break;
                    if (match.Value.Contains("Warp")) break;
                    //RandoMapMod.Instance.Log($"{match.Value} {match.Value.L()}");
                    break;
            }
#endif
            /*
            if(!Matchs.Contains(match))
                Matchs.Add(match);
            On.HeroController.Update -= HeroController_Update;
            On.HeroController.Update += HeroController_Update;*/
            return match.Value.L();
        }
        /*
        private static void HeroController_Update(On.HeroController.orig_Update orig, HeroController self)
        {
            orig(self);
            LogMatchs();
        }

        public static List<Match> Matchs = new ();
        public static void LogMatchs()
        {
            if (Matchs.Count == 0) return;
            var match = Matchs[0];
            if (RandomizerMod.RandomizerMod.RS.TrackerData.lm.LogicLookup.TryGetValue("Greenpath_Stag", out var ld))
                RandoMapMod.Instance.Log($"l {"Greenpath_Stag"}: {ld.InfixSource.LL()}");
            //if (RandomizerMod.RandomizerMod.RS.TrackerData.lm.TransitionLookup.TryGetValue(match.Value, out var td))
                //RandoMapMod.Instance.Log($"t {match.Value}: {td.logic.InfixSource.LL()}");
            //if (RandomizerMod.RandomizerMod.RS.TrackerData.lm.ItemLookup.TryGetValue(match.Value, out var id))
                //RandoMapMod.Instance.Log($"i {match.Value}: {id.Name.LL()}");
            if (RandomizerMod.RandomizerMod.RS.TrackerData.lm.LP.IsMacro(match.Value))
            {
                var lpms = RandomizerMod.RandomizerMod.RS.TrackerData.lm.LP.GetMacro(match.Value);
                //foreach (var l in lpms)
                RandoMapMod.Instance.Log($"lp {match.Value}: {lpms.ToInfix().Replace("[","'").LL()}");
            }
            Matchs.Remove(match);
        }*/
    }
}