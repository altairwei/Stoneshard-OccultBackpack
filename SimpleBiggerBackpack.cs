﻿// Copyright (C)
// See LICENSE file for extended copyright information.
// This file is part of the repository from .

using System;
using System.Collections.Generic;
using System.Linq;

using ModShardLauncher;
using ModShardLauncher.Mods;
using UndertaleModLib;
using UndertaleModLib.Models;

namespace SimpleBiggerBackpack;
public class SimpleBiggerBackpack : Mod
{
    public override string Author => "Altair";
    public override string Name => "Simple Bigger Backpack";
    public override string Description => "Now the tailor of Osbrook will sell a bigger backpack.";
    public override string Version => "1.0.0";
    public override string TargetVersion => "0.8.2.10";

    public override void PatchMod()
    {
        Msl.AddFunction(ModFiles.GetCode("scr_msl_debug.gml"), "scr_msl_debug");

        // Create the container of Backpack
        UndertaleGameObject o_container_masterpiecebackpack = Msl.AddObject(
            name: "o_container_masterpiecebackpack",
            spriteName: "s_container", 
            parentName: "o_container_backpack",
            isVisible: true, 
            isAwake: true
        );

        o_container_masterpiecebackpack.ApplyEvent(ModFiles, 
            new MslEvent("gml_Object_o_container_masterpiecebackpack_Other_10.gml", EventType.Other, 10)
        );

        int index = DataLoader.data.GameObjects.IndexOf(DataLoader.data.GameObjects.First(x => x.Name.Content == "o_container_backpack"));
        Msl.LoadGML("gml_GlobalScript_scr_adaptiveCloseButtonCreate")
            .MatchFrom($"                case {index}:")
            .InsertBelow("                case o_container_masterpiecebackpack:")
            .Save();

        Msl.LoadGML("gml_GlobalScript_scr_adaptiveTakeAllButtonCreate")
            .MatchFrom($"                case {index}:")
            .InsertBelow("                case o_container_masterpiecebackpack:")
            .Save();

        Msl.LoadGML("gml_GlobalScript_scr_adaptiveMenusGetOffset")
            .MatchFrom($"        case {index}:")
            .InsertBelow("        case o_container_masterpiecebackpack:")
            .Save();

        Msl.LoadGML("gml_Object_o_inv_slot_Mouse_4")
            .MatchFrom($"                    case {index}:")
            .InsertBelow("                    case o_container_masterpiecebackpack:")
            .Save();

        Msl.LoadGML("gml_Object_o_inv_slot_Other_13")
            .MatchFrom($"                case {index}:")
            .InsertBelow("                case o_container_masterpiecebackpack:")
            .Save();

        /* Don't patch gml_GlobalScript_scr_adaptiveMenusPositionUpdate,
         * I moved the relevant codes to gml_Object_o_container_masterpiecebackpack_Other_10.gml
        int index_o_container = DataLoader.data.GameObjects.IndexOf(DataLoader.data.GameObjects.First(x => x.Name.Content == "o_container"));
        Msl.LoadGML("gml_GlobalScript_scr_adaptiveMenusPositionUpdate")
            .MatchFrom($"        case {index_o_container}:")
            .InsertBelow("        case o_container_masterpiecebackpack:")
            .Save();

        // FIXME: Make Character Menu Crash!
        Msl.LoadGML("gml_GlobalScript_scr_adaptiveMenusPositionUpdate")
            .MatchFrom("            scr_guiLayoutOffsetUpdate(id, ((-sprite_width) * (!active)))")
            //.ReplaceBy("            scr_msl_debug(string(id) + \", \" + string(-sprite_width) + \" * \" + string(!active))")
            .ReplaceBy("")
            .Peek()
            .Save();
        */

        Msl.LoadGML("gml_GlobalScript_scr_can_replace_item")
            .MatchFrom("        if (argument0.is_open && argument1.owner.object_index == o_container_backpack)")
            .ReplaceBy("        if (argument0.is_open && (argument1.owner.object_index == o_container_backpack || object_is_ancestor(argument1.owner.object_index, o_container_backpack)))")
            .Save();

        Msl.LoadGML("gml_GlobalScript_scr_can_replace_item")
            .MatchFrom("        if (argument0.is_open && argument1.object_index == o_container_backpack)")
            .ReplaceBy("        if (argument0.is_open && (argument1.object_index == o_container_backpack || object_is_ancestor(argument1.object_index, o_container_backpack)))")
            .Save();

        // Create Backpack
        UndertaleSprite s_masterpiecebackpack = Msl.GetSprite("s_inv_masterpiecebackpack");
        s_masterpiecebackpack.IsSpecialType = true;
        s_masterpiecebackpack.SVersion = 3;
        s_masterpiecebackpack.Width = 54;
        s_masterpiecebackpack.Height = 81;
        s_masterpiecebackpack.MarginLeft = 1;
        s_masterpiecebackpack.MarginRight = 53;
        s_masterpiecebackpack.MarginBottom = 80;
        s_masterpiecebackpack.MarginTop = 1;

        UndertaleGameObject o_inv_masterpiecebackpack = Msl.AddObject(
            name: "o_inv_masterpiecebackpack",
            spriteName: "s_inv_masterpiecebackpack", 
            parentName: "o_inv_backpack",
            isVisible: true, 
            isPersistent: true,
            isAwake: true
        );

        Msl.InjectTableItemLocalization(
            oName: "masterpiecebackpack",
            dictName: new Dictionary<ModLanguage, string>() {
                {ModLanguage.English, "Masterpiece Backpack"},
                {ModLanguage.Chinese, "匠作背包"}
            },
            dictID: new Dictionary<ModLanguage, string>() {
                {ModLanguage.English, "A exquisite backpack that's smaller and holds more stuff."},
                {ModLanguage.Chinese, "一个更加小巧能装更多的东西的背包。"}
            },
            dictDescription: new Dictionary<ModLanguage, string>() {
                {ModLanguage.English, "The masterpiece of the Osbrook tailor. While the style is similar to a traditional backpack, this one is more compact and can hold more."},
                {ModLanguage.Chinese, "奥斯布鲁克裁缝的呕心沥血之作。虽然款式与传统背包差不多，但这个背包更加小巧，能装更多的东西。"}
            }
        );

        o_inv_masterpiecebackpack.ApplyEvent(ModFiles,
            new MslEvent("gml_Object_o_inv_masterpiecebackpack_Create_0.gml", EventType.Create, 0),
            new MslEvent("gml_Object_o_inv_masterpiecebackpack_Other_24.gml", EventType.Other, 24)
        );

        // Create the loot object of Backpack
        UndertaleGameObject o_loot_masterpiecebackpack = Msl.AddObject(
            name: "o_loot_masterpiecebackpack",
            spriteName: "s_loot_travellersbackpack", 
            parentName: "o_loot_backpack",
            isVisible: true, 
            isAwake: true
        );

        o_loot_masterpiecebackpack.ApplyEvent(ModFiles, 
            new MslEvent("gml_Object_o_loot_masterpiecebackpack_Create_0.gml", EventType.Create, 0)
        );

        // Add the interaction between Backpack and other items in the o_inv_slot
        // FIXME: We had to replace the entire file, otherwise it would have resulted in messed up lines of code
        Msl.LoadGML("gml_Object_o_inv_slot_Other_21")
            .MatchAll()
            .ReplaceBy(ModFiles, "gml_Object_o_inv_slot_Other_21.gml")
            .Save();

        Msl.LoadGML("gml_GlobalScript_scr_gold_count")
            .MatchFrom("            if (owner.object_index == o_inventory || owner.object_index == o_container_backpack)")
            .ReplaceBy("            if (owner.object_index == o_inventory || owner.object_index == o_container_backpack || object_is_ancestor(owner.object_index, o_container_backpack))")
            .Save();
        Msl.LoadGML("gml_GlobalScript_scr_gold_count")
            .MatchFrom("            if (owner.object_index == o_inventory || owner.object_index == o_container_backpack)")
            .ReplaceBy("            if (owner.object_index == o_inventory || owner.object_index == o_container_backpack || object_is_ancestor(owner.object_index, o_container_backpack))")
            .Save();
        Msl.LoadGML("gml_GlobalScript_scr_gold_count")
            .MatchFrom("                if (owner.object_index == o_inventory || owner.object_index == o_container_backpack)")
            .ReplaceBy("                if (owner.object_index == o_inventory || owner.object_index == o_container_backpack || object_is_ancestor(owner.object_index, o_container_backpack))")
            .Save();

        Msl.LoadGML("gml_Object_o_inv_slot_Destroy_0")
            .MatchFrom("    if (owner.object_index != o_container_backpack)")
            .ReplaceBy("    if (owner.object_index != o_container_backpack && !object_is_ancestor(owner.object_index, o_container_backpack))")
            .Save();

        // FIXME: We had to replace the entire file, otherwise it would have resulted in messed up lines of code
        /*
        Msl.LoadGML("gml_GlobalScript_scr_notify")
            .MatchAll()
            .ReplaceBy(ModFiles, "gml_GlobalScript_scr_notify.gml")
            .Save();
        */

        // Add backpacks to the goods of the Osbrook tailor
        //Msl.LoadGML("gml_Object_o_npc_tailor_Create_0")
        //    .MatchFrom("ds_list_add(selling_loot_object, 2689, 2.5, 2926, 2.5, 2931, 2.5, 3088, 2.5)")
        //    .InsertBelow("ds_list_add(selling_loot_object, 2936, 2.5)") // backpack: inv_object = 2936
        //    .Save();

        // Init the mini quest of backpack making (only works in a new game save)
        Msl.LoadGML("gml_GlobalScript_scr_init_quests")
            .MatchFrom("scr_quest_init(\"fetchOrmond\", \"\", [\"fetchOrmond_find\", 1, \"fetchOrmond_desc\", []])")
            .InsertBelow("scr_quest_init(\"makeBackpackOrmond\", \"\", [\"makeBackpackOrmond_find\", 1, \"makeBackpackOrmond_desc\", []])")
            .Save();
        
        // Add quest name and description
        Msl.LoadGML("gml_GlobalScript_table_Quests_text").Apply(QeustIterator).Save();

        // Add dialogs to questions map
        Msl.LoadGML("gml_Object_o_npc_tailor_Alarm_1").MatchAll()
            .InsertBelow(ModFiles.GetCode("gml_Object_o_npc_tailor_Alarm_1.gml")).Save();
        // Add dialog texts
        Msl.InjectTableDialogLocalization(
            new LocalizationSentence(
                "tailor_backpack_pc",
                new Dictionary<ModLanguage, string>() {
                    {ModLanguage.English, "How come you don't sell backpacks here?"},
                    {ModLanguage.Chinese, "你这里怎么没有卖背包？"}
                }
            ),
            new LocalizationSentence(
                "tailor_backpack_inquiry",
                new Dictionary<ModLanguage, string>() {
                    {ModLanguage.English, string.Join("#", new string[] {
                        "A backpack? Ha ha ha ha. Who's going to buy a backpack when Osbrook is this big?",
                        "The war is so chaotic right now, no one dares to travel far, so even more so, no one would buy one. ",
                        "Aldor's traditional travelling backpacks are so big and bulky that they're not practical at all, and only those inexperienced rookies would buy them. I'm the one who won't make this rubbish."
                    })},
                    {ModLanguage.Chinese, string.Join("#", new string[] {
                        "背包？哈哈哈哈。奥村就这么大点，谁会买背包啊？况且现在战争这么乱，谁也不敢出远门，更没人买了。",
                        "奥尔多传统的旅行背包又大又笨重，根本不实用，只有那些没经验的菜鸟会买。我才会不会制作这种垃圾。"
                    })}
                }
            ),
            new LocalizationSentence(
                "tailor_backpack_inquiry_pc",
                new Dictionary<ModLanguage, string>() {
                    {ModLanguage.English, "Does that mean you can make a small and functional backpack?"},
                    {ModLanguage.Chinese, "那意思是你能制作一个小巧又实用的背包？"}
                }
            ),
            new LocalizationSentence(
                "tailor_backpack_quest",
                new Dictionary<ModLanguage, string>() {
                    {ModLanguage.English, string.Join("#", new string[] {
                        "Since you've asked, I must demonstrate my ancestral craft. But at the moment I don't have the right materials on hand.",
                        "This way, you find a pelt, a bolt of cloth, and a spool of thread. I'll only charge you 50 craft fee to make you a exquisite backpack."
                    })},
                    {ModLanguage.Chinese, string.Join("#", new string[] {
                        "既然你都这么问了，我必须展示祖传的手艺了。但目前我手上没有合适的材料。",
                        "这样，你找到一张动物毛皮，一卷布，以及一轴毛线。我只收你 50 冠手工费，给你制作一个精致的背包。"
                    })}
                }
            ),
            new LocalizationSentence(
                "backpack_materials_collected",
                new Dictionary<ModLanguage, string>() {
                    {ModLanguage.English, "I've collected all the materials for making a backpack."},
                    {ModLanguage.Chinese, "制作背包的材料我都收集好了。"}
                }
            ),
            new LocalizationSentence(
                "tailor_making_backpack",
                new Dictionary<ModLanguage, string>() {
                    {ModLanguage.English, "OK, backpacks will be ready for you in a minute."},
                    {ModLanguage.Chinese, "行，马上就为你做好背包。"}
                }
            )
        );

        // Change dialog to add a mini quest
        Msl.AddFunction(ModFiles.GetCode("gml_GlobalScript_scr_npc_tailor_backpack_reward.gml"), "scr_npc_tailor_backpack_reward");
        Msl.LoadGML("gml_GlobalScript_scr_npc_miniquest_item_tailor")
            .MatchAll()
            .ReplaceBy(ModFiles.GetCode("gml_GlobalScript_scr_npc_miniquest_item_tailor.gml"))
            .Save();

    }

    private static IEnumerable<string> QeustIterator(IEnumerable<string> input)
    {
        string id = "makeBackpackOrmond";
        string text_en = @"Hold the Tailor's Backpack Making";
        string text_zh = @"裁缝霍特背包制作";
        string makeBackpackOrmond = $"{id};{text_en};{text_en};{text_zh};" + string.Concat(Enumerable.Repeat($"{text_en};", 9));

        id = "makeBackpackOrmond_find";
        text_en = @"Find a Pelt, a Bolt of Cloth, and a Spool of Thread";
        text_zh = @"寻找毛皮、亚麻布和毛线";
        string makeBackpackOrmond_find = $"{id};{text_en};{text_en};{text_zh};" + string.Concat(Enumerable.Repeat($"{text_en};", 9));

        id = "makeBackpackOrmond_desc";
        text_en = @"Hold the Tailor from Osbrook doesn't sell backpacks, but is willing to help me make a new one. He asked me to get him a pelt, a bolt of cloth, and a spool of thread, plus fifty crowns for the craft.";
        text_zh = @"奥斯布鲁克的裁缝霍特不卖背包，但愿意帮我制作一个新的。他让我给他弄一张毛皮、一卷亚麻布和一轴毛线，再加上五十冠的手工费。";
        string makeBackpackOrmond_desc = $"{id};{text_en};{text_en};{text_zh};" + string.Concat(Enumerable.Repeat($"{text_en};", 9));

        string questend = "\";" + string.Concat(Enumerable.Repeat("text_end;", 12)) + "\"";

        foreach(string item in input)
        {
            if(item.Contains(questend))
            {
                string newItem = item;
                newItem = newItem.Insert(newItem.IndexOf(questend), $"\"{makeBackpackOrmond}\",\"{makeBackpackOrmond_find}\",\"{makeBackpackOrmond_desc}\",");
                yield return newItem;
            }
            else
            {
                yield return item;
            }
        }
    }
}
