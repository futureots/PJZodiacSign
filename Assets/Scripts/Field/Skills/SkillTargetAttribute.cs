using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTargetAttribute : Attribute
{
    string text;
    
    public SkillTargetAttribute(string text = "")
    {
        this.text = text;
    }
}
