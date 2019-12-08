using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class InfoPanel : UIBaseBehaviour
{
    public TextMeshProUGUI title;

    public abstract void RefreshContent(SlotElement slotElement);
}
