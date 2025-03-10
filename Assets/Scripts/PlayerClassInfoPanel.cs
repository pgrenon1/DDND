﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerClassInfoPanel : UIBaseBehaviour
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI description;

    public void RefreshContent(PlayerClass playerClass)
    {
        title.text = playerClass.className;
    }
}
