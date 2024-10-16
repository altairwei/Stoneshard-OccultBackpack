event_inherited()
if (!string_length(nameCaps))
    nameCaps = scr_stringTransform(name, true)
scr_drawMenuTitle((x + adaptiveTitleOffsetX + sprite_width / 2), (y + adaptiveTitleOffsetY), nameCaps)
