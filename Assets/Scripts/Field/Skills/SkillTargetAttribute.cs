using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTargetAttribute : Attribute
{
    public string text {  get; private set; }
    
    public SkillTargetAttribute(string text = "")
    {
        this.text = text;
    }
}
