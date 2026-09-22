using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class LevelManager
{
    private static readonly String menuScene = "Menu";
    private static readonly String gameScene = "Level";
    private static readonly String miniGameScene = "Questionario";
    private static readonly LevelAtributes[] atributesLevel = new LevelAtributes[]
    {
        new LevelAtributes {energySpend = 1}
    };
    private static int level = 0;

    public static void StartGame() {
        level = 0;
        NextLevel();
        //MinigameLevel();
    }
    public static void NextLevel() {
        level += 1;
        SceneManager.LoadScene(gameScene);
    }
    public static void MinigameLevel() {
        SceneManager.LoadScene(miniGameScene);
    }
    public static void ToGoHomeScreen() {
        level = 0;
        SceneManager.LoadScene(menuScene);
    }
}

public struct LevelAtributes {
    public float energySpend;
}
