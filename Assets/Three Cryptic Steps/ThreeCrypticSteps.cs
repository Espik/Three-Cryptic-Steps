using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KModkit;

public class ThreeCrypticSteps : MonoBehaviour {
    public KMAudio Audio;
    public KMBombInfo Bomb;
    public KMBombModule Module;
    public KMColorblindMode ColorblindMode;

    public KMSelectable[] Keys;
    public MeshRenderer[] KeyModels;
    public TextMesh[] KeyTexts;
    public TextMesh ScreenText;
    public Material[] KeyMaterials;
    public Color[] ScreenColors;
    public TextMesh[] NumberTexts;

    // Solving info
    private int stage = 1;
    private int moduleStrikes = 0;
    private bool canPress = true;

    private int[] keyState = new int[25];
    private bool[] validSerialNumbers = new bool[10];

    private int validSerialNumberCount = 0;
    private int validNumbersFound = 0;
    private bool[] numberFound = new bool[10];
    private int[] numberColors = new int[10];

    private int[] colorCount = new int[6];
    private int presentColors = 0;
    private bool positionChecked = true;

    private bool allowTenMinCheck = true;

    private string enteredText = "";
    private string submitText = "";
    private int enteredLength = 0;

    private bool enteredPassword = false;
    private bool enteredOutburst = false;
    private bool enteredBasketball = false;
    private bool enteredSandwhich = false;

    private int solveMarker = 52;
    private bool isVirtual = false;
    private bool shownPassword = false;

    private string lastSolvedModule = "Undefined";
    private int serialDigitSum = 0;

    private int[] easyNumbers = { 0, 1, 4, 7 };
    private int[] hardNumbers = { 2, 3, 5, 6, 8, 9 };
    private int serialIndex = 0;
    private bool hardNumberFound = false;

    // Logging info
    private static int moduleIdCounter = 1;
    private int moduleId;

    // Stage 1 Valid Numbers
    private readonly int[] validGreenNumbers = { 7, 11, 17, 23, 41, 47, 71, 83, 101, 107, 113, 131, 167, 197, 227, 233, 281, 311, 317, 353, 383,
        401, 443, 461, 467, 491, 503 };

    private readonly int[] validRedNumbers = { 25, 39, 49, 55, 69, 81, 85, 91, 95, 99, 115, 119, 121, 123, 125, 129, 133, 145, 147, 159, 165, 169,
        175, 187, 189, 205, 207, 209, 213, 217, 219, 221, 225, 235, 247, 249, 253, 259, 265, 269, 273, 279, 289, 291, 295, 299, 301, 303, 305, 309,
        319, 323, 325, 327, 329, 333, 339, 343, 345, 355, 361, 365, 369, 375, 381, 385, 391, 395, 399, 403, 407, 411, 415, 417, 427, 429, 441, 445,
        451, 455, 459, 469, 473, 475, 477, 481, 485, 489, 493, 497, 501, 505, 511, 515, 517, 519, 529, 531, 533, 535, 537, 539, 543, 549, 553, 555,
        559, 565, 579, 583, 585, 589, 595};

    // Stage 2 Animation
    private readonly int[] group1 = { 0, 1, 2, 5, 6, 7, 10, 11, 12 };
    private readonly int[] group2 = { 1, 2, 3, 6, 7, 8, 11, 12, 13 };
    private readonly int[] group3 = { 2, 3, 4, 7, 8, 9, 12, 13, 14 };
    private readonly int[] group4 = { 7, 8, 9, 12, 13, 14, 17, 18, 19 };
    private readonly int[] group5 = { 12, 13, 14, 17, 18, 19, 22, 23, 24 };
    private readonly int[] group6 = { 11, 12, 13, 16, 17, 18, 21, 22, 23 };
    private readonly int[] group7 = { 10, 11, 12, 15, 16, 17, 20, 21, 22 };
    private readonly int[] group8 = { 5, 6, 7, 10, 11, 12, 15, 16, 17 };

    // Stage 2 Number Configuations
    private readonly int[] number0 = { 0, 1, 2, 3, 4, 5, 9, 10, 14, 15, 19, 20, 21, 22, 23, 24 };
    private readonly int[] number1 = { 2, 7, 12, 17, 22 };
    private readonly int[] number2 = { 0, 1, 2, 3, 4, 9, 10, 11, 12, 13, 14, 15, 20, 21, 22, 23, 24 };
    private readonly int[] number3 = { 0, 1, 2, 3, 4, 9, 10, 11, 12, 13, 14, 19, 20, 21, 22, 23, 24 };
    private readonly int[] number4 = { 0, 4, 5, 9, 10, 11, 12, 13, 14, 19, 24 };
    private readonly int[] number5 = { 0, 1, 2, 3, 4, 5, 10, 11, 12, 13, 14, 19, 20, 21, 22, 23, 24 };
    private readonly int[] number6 = { 0, 1, 2, 3, 4, 5, 10, 11, 12, 13, 14, 15, 19, 20, 21, 22, 23, 24 };
    private readonly int[] number7 = { 0, 1, 2, 3, 4, 9, 14, 19, 24 };
    private readonly int[] number8 = { 0, 1, 2, 3, 4, 5, 9, 10, 11, 12, 13, 14, 15, 19, 20, 21, 22, 23, 24 };
    private readonly int[] number9 = { 0, 1, 2, 3, 4, 5, 9, 10, 11, 12, 13, 14, 19, 20, 21, 22, 23, 24 };

    private readonly int[] notNumber0 = { 6, 7, 8, 11, 12, 13, 16, 17, 18 };
    private readonly int[] notNumber1 = { 0, 1, 3, 4, 5, 6, 8, 9, 10, 11, 13, 14, 15, 16, 18, 19, 20, 21, 23, 24 };
    private readonly int[] notNumber2 = { 5, 6, 7, 8, 16, 17, 18, 19 };
    private readonly int[] notNumber3 = { 5, 6, 7, 8, 15, 16, 17, 18 };
    private readonly int[] notNumber4 = { 1, 2, 3, 6, 7, 8, 15, 16, 17, 18, 20, 21, 22, 23 };
    private readonly int[] notNumber5 = { 6, 7, 8, 9, 15, 16, 17, 18 };
    private readonly int[] notNumber6 = { 6, 7, 8, 9, 16, 17, 18 };
    private readonly int[] notNumber7 = { 5, 6, 7, 8, 10, 11, 12, 13, 15, 16, 17, 18, 20, 21, 22, 23 };
    private readonly int[] notNumber8 = { 6, 7, 8, 16, 17, 18 };
    private readonly int[] notNumber9 = { 6, 7, 8, 15, 16, 17, 18 };

    // Stage 3 Letter Configuration
    private readonly string[] keyLetters = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "-", "V", "W", "Y", "*" };
    private readonly string[] majorLetters = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "Y" };

    private readonly char[] capitalLetters = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
    private readonly char[] lowercaseLetters = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };

    // Colorblind Mode
    private bool colorblindMode = false;
    private readonly string[] colorblindLetters = { "R", "Y", "G", "C", "B", "M" };

    // Virtuallion Modules
    private readonly string[] virtualModules = { "...?", "1000 Words", "Adventure Game", "Alphabet", "A Mistake", "Anagrams", "Arrow Talk", "Bamboozled Again", "Binary",
        "Bone Apple Tea", "Boolean Venn Diagram", "Broken Buttons", "The Bulb", "The Button", "Button Sequence", "The Clock", "Colored Squares", "Color Generator", "Complex Keypad",
        "Complicated Wires", "Countdown", "Crazy Talk", "Creation", "Digital Cipher", "Digital Root", "Directional Button", "Double Color", "Double-Oh", "Encrypted Dice", "Extended Password",
        "Follow the Leader", "Foreign Exchange Rates", "Forget Me Not", "The Gamepad", "Gatekeeper", "Green Arrows", "Grid Matching", "The iPhone", "The Jack-O'-Lantern", "Keypad",
        "The Labyrinth", "Lasers", "Letter Keys", "Light Cycle", "Lucky Dice", "Mashematics", "Mastermind Simple", "Maze", "Maze Scrambler", "Memory", "Microcontroller", "Modulo",
        "Monsplode, Fight!", "Morse Code", "Not the Button", "Not Complicated Wires", "Not Keypad", "Not Maze", "Not Memory", "Not Who's on First", "The Number Cipher", "Orange Arrows",
        "Password", "Piano Keys", "Pictionary", "The Plunger Button", "Poetry", "Poker", "Prime Checker", "Red Arrows", "Resistors", "Rock-Paper-Scissors-L.-Sp.", "Round Keypad",
        "The Rule", "The Screw", "S.E.T.", "Simon Says", "Simon Scrambles", "Simon States", "Sink", "Souvenir", "Square Button", "Switches", "Symbolic Coordinates", "Synonyms",
        "Tasha Squeals", "Text Field", "Three Cryptic Steps", "Tic Tac Toe", "Timezone", "Two Bits", "Uncolored Squares", "Unrelated Anagrams", "USA Maze", "Visual Impairment",
        "Who's on First", "Wire Placement", "Wires", "Wire Sequence", "Word Scramble", "Yahtzee" };


    // Ran as bomb loads
    private void Awake() {
        moduleId = moduleIdCounter++;

        for (int i = 0; i < Keys.Length; i++) {
            int j = i;
            Keys[i].OnInteract += delegate () { KeyPressed(j); return false; };
        }
	}

    // Gets information
    private void Start() {
        // New method - Picks random number from 0,1,4,7 not to use, all bonus numbers must be present in the serial number
        serialIndex = Bomb.GetSerialNumberNumbers().Sum() % 4;
        validSerialNumberCount = 3;

        for (int i = 0; i < easyNumbers.Length; i++) {
            if (i != serialIndex) validSerialNumbers[easyNumbers[i]] = true;
        }

        for (int i = 0; i < hardNumbers.Length; i++) {
            if (Bomb.GetSerialNumberNumbers().Contains(hardNumbers[i])) validSerialNumbers[hardNumbers[i]] = true;
        }
        
        // Old method - Required all numbers present of the serial number
        /*for (int i = 0; i < validSerialNumbers.Length; i++) {
            if (Bomb.GetSerialNumberNumbers().Contains(i)) {
                validSerialNumbers[i] = true;
                validSerialNumberCount++;
            }

            else
                validSerialNumbers[i] = false;
        }*/

        for (int i = 0; i < numberFound.Length; i++) {
            numberFound[i] = false;
            numberColors[i] = 8;
        }

        serialDigitSum = Bomb.GetSerialNumberNumbers().Sum();
        colorblindMode = ColorblindMode.ColorblindModeActive;

        // Virtuallion
        solveMarker = UnityEngine.Random.Range(52, 87);
        var m = Bomb.GetSolvableModuleNames();
        
        if (m.Count() == 101) {
            int successCount = 0;

            for (int i = 0; i < virtualModules.Length; i++) {
                if (m.Count(x => x.Contains(virtualModules[i])) >= 1) {
                    successCount++;
                }
            }

            if (successCount == 101) {
                isVirtual = true;
                Debug.LogFormat("[Three Cryptic Steps #{0}] Virtuallion detected.", moduleId);
            }
        }
    }


    // Key pressed
    private void KeyPressed(int i) {
        Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
        Keys[i].AddInteractionPunch(0.25f);

        if (canPress == true) {
            // Stage 1
            if (stage == 1) {
                // 32 Minutes
                if (Bomb.GetTime() >= 1920.0f && Bomb.GetTime() < 1980.0f) {
                    if (i == 13) {
                        Debug.LogFormat("[Three Cryptic Steps #{0}] The right button was pressed at {1}.", moduleId, Bomb.GetFormattedTime());
                        canPress = false;
                        StartCoroutine(StageOneAdvance());
                    }

                    else if (i == 11) {
                        Debug.LogFormat("[Three Cryptic Steps #{0}] The left button was pressed at {1}.", moduleId, Bomb.GetFormattedTime());
                        canPress = false;
                        StartCoroutine(StageOneStrike());
                    }
                }
                
                else {
                    // Right button
                    if (i == 13) {
                        Debug.LogFormat("[Three Cryptic Steps #{0}] The right button was pressed at {1}.", moduleId, Bomb.GetFormattedTime());
                        canPress = false;
                        int value = (int) Bomb.GetTime() % 600;
                        bool valid = false;
                        
                        for (int j = 0; j < validGreenNumbers.Length; j++) {
                            if (validGreenNumbers[j] == value) {
                                valid = true;
                                break;
                            }
                        }

                        if (valid == true)
                            StartCoroutine(StageOneAdvance());

                        else
                            StartCoroutine(StageOneStrike());
                    }

                    // Left button
                    else if (i == 11) {
                        Debug.LogFormat("[Three Cryptic Steps #{0}] The left button was pressed at {1}.", moduleId, Bomb.GetFormattedTime());
                        canPress = false;
                        int value = (int) Bomb.GetTime() % 600;
                        bool valid = false;

                        for (int j = 0; j < validRedNumbers.Length; j++) {
                            if (validRedNumbers[j] == value) {
                                valid = true;
                                break;
                            }
                        }

                        if (valid == true)
                            StartCoroutine(StageOneAdvance());

                        else
                            StartCoroutine(StageOneStrike());
                    }
                }
            }

            // Stage 2
            else if (stage == 2) {
                switch (keyState[i]) {
                case 0: // Red

                if (i > 4) { // Up
                    keyState[i - 5]++;
                    keyState[i - 5] %= 6;
                    KeyModels[i - 5].material = KeyMaterials[keyState[i - 5]];
                    if (colorblindMode == true) KeyTexts[i - 5].text = colorblindLetters[keyState[i - 5]];
                }

                if (i % 5 != 4) { // Right
                    keyState[i + 1]++;
                    keyState[i + 1] %= 6;
                    KeyModels[i + 1].material = KeyMaterials[keyState[i + 1]];
                    if (colorblindMode == true) KeyTexts[i + 1].text = colorblindLetters[keyState[i + 1]];
                }

                if (i < 20) { // Down
                    keyState[i + 5]++;
                    keyState[i + 5] %= 6;
                    KeyModels[i + 5].material = KeyMaterials[keyState[i + 5]];
                    if (colorblindMode == true) KeyTexts[i + 5].text = colorblindLetters[keyState[i + 5]];
                }

                if (i % 5 != 0) { // Left
                    keyState[i - 1]++;
                    keyState[i - 1] %= 6;
                    KeyModels[i - 1].material = KeyMaterials[keyState[i - 1]];
                    if (colorblindMode == true) KeyTexts[i - 1].text = colorblindLetters[keyState[i - 1]];
                }

                break;

                case 1: // Yellow

                if (i > 4 && i % 5 != 0) { // Up-left
                    keyState[i - 6]++;
                    keyState[i - 6] %= 6;
                    KeyModels[i - 6].material = KeyMaterials[keyState[i - 6]];
                    if (colorblindMode == true) KeyTexts[i - 6].text = colorblindLetters[keyState[i - 6]];
                }

                if (i > 4 && i % 5 != 4) { // Up-right
                    keyState[i - 4]++;
                    keyState[i - 4] %= 6;
                    KeyModels[i - 4].material = KeyMaterials[keyState[i - 4]];
                    if (colorblindMode == true) KeyTexts[i - 4].text = colorblindLetters[keyState[i - 4]];
                }

                if (i < 20 && i % 5 != 4) { // Down-right
                    keyState[i + 6]++;
                    keyState[i + 6] %= 6;
                    KeyModels[i + 6].material = KeyMaterials[keyState[i + 6]];
                    if (colorblindMode == true) KeyTexts[i + 6].text = colorblindLetters[keyState[i + 6]];
                }

                if (i < 20 && i % 5 != 0) { // Down-left
                    keyState[i + 4]++;
                    keyState[i + 4] %= 6;
                    KeyModels[i + 4].material = KeyMaterials[keyState[i + 4]];
                    if (colorblindMode == true) KeyTexts[i + 4].text = colorblindLetters[keyState[i + 4]];
                }

                break;

                case 2: // Green

                if (i > 4 && i % 5 != 0) { // Up-left
                    keyState[i - 6]++;
                    keyState[i - 6] %= 6;
                    KeyModels[i - 6].material = KeyMaterials[keyState[i - 6]];
                    if (colorblindMode == true) KeyTexts[i - 6].text = colorblindLetters[keyState[i - 6]];
                }

                if (i > 4) { // Up
                    keyState[i - 5]++;
                    keyState[i - 5] %= 6;
                    KeyModels[i - 5].material = KeyMaterials[keyState[i - 5]];
                    if (colorblindMode == true) KeyTexts[i - 5].text = colorblindLetters[keyState[i - 5]];
                }

                if (i > 4 && i % 5 != 4) { // Up-right
                    keyState[i - 4]++;
                    keyState[i - 4] %= 6;
                    KeyModels[i - 4].material = KeyMaterials[keyState[i - 4]];
                    if (colorblindMode == true) KeyTexts[i - 4].text = colorblindLetters[keyState[i - 4]];
                }

                if (i < 20 && i % 5 != 0) { // Down-left
                    keyState[i + 4]++;
                    keyState[i + 4] %= 6;
                    KeyModels[i + 4].material = KeyMaterials[keyState[i + 4]];
                    if (colorblindMode == true) KeyTexts[i + 4].text = colorblindLetters[keyState[i + 4]];
                }

                if (i < 20) { // Down
                    keyState[i + 5]++;
                    keyState[i + 5] %= 6;
                    KeyModels[i + 5].material = KeyMaterials[keyState[i + 5]];
                    if (colorblindMode == true) KeyTexts[i + 5].text = colorblindLetters[keyState[i + 5]];
                }

                if (i < 20 && i % 5 != 4) { // Down-right
                    keyState[i + 6]++;
                    keyState[i + 6] %= 6;
                    KeyModels[i + 6].material = KeyMaterials[keyState[i + 6]];
                    if (colorblindMode == true) KeyTexts[i + 6].text = colorblindLetters[keyState[i + 6]];
                }

                break;

                case 3: // Cyan

                if (i > 4 && i % 5 != 0) { // Up-left
                    keyState[i - 6]++;
                    keyState[i - 6] %= 6;
                    KeyModels[i - 6].material = KeyMaterials[keyState[i - 6]];
                    if (colorblindMode == true) KeyTexts[i - 6].text = colorblindLetters[keyState[i - 6]];
                }

                if (i % 5 != 0) { // Left
                    keyState[i - 1]++;
                    keyState[i - 1] %= 6;
                    KeyModels[i - 1].material = KeyMaterials[keyState[i - 1]];
                    if (colorblindMode == true) KeyTexts[i - 1].text = colorblindLetters[keyState[i - 1]];
                }

                if (i < 20 && i % 5 != 0) { // Down-left
                    keyState[i + 4]++;
                    keyState[i + 4] %= 6;
                    KeyModels[i + 4].material = KeyMaterials[keyState[i + 4]];
                    if (colorblindMode == true) KeyTexts[i + 4].text = colorblindLetters[keyState[i + 4]];
                }

                if (i > 4 && i % 5 != 4) { // Up-right
                    keyState[i - 4]++;
                    keyState[i - 4] %= 6;
                    KeyModels[i - 4].material = KeyMaterials[keyState[i - 4]];
                    if (colorblindMode == true) KeyTexts[i - 4].text = colorblindLetters[keyState[i - 4]];
                }

                if (i % 5 != 4) { // Right
                    keyState[i + 1]++;
                    keyState[i + 1] %= 6;
                    KeyModels[i + 1].material = KeyMaterials[keyState[i + 1]];
                    if (colorblindMode == true) KeyTexts[i + 1].text = colorblindLetters[keyState[i + 1]];
                }

                if (i < 20 && i % 5 != 4) { // Down-right
                    keyState[i + 6]++;
                    keyState[i + 6] %= 6;
                    KeyModels[i + 6].material = KeyMaterials[keyState[i + 6]];
                    if (colorblindMode == true) KeyTexts[i + 6].text = colorblindLetters[keyState[i + 6]];
                }

                break;

                case 4: // Blue

                if (i > 4 && i % 5 != 0) { // Up-left
                    keyState[i - 6]++;
                    keyState[i - 6] %= 6;
                    KeyModels[i - 6].material = KeyMaterials[keyState[i - 6]];
                    if (colorblindMode == true) KeyTexts[i - 6].text = colorblindLetters[keyState[i - 6]];
                }

                if (i > 4) { // Up
                    keyState[i - 5]++;
                    keyState[i - 5] %= 6;
                    KeyModels[i - 5].material = KeyMaterials[keyState[i - 5]];
                    if (colorblindMode == true) KeyTexts[i - 5].text = colorblindLetters[keyState[i - 5]];
                }

                if (i > 4 && i % 5 != 4) { // Up-right
                    keyState[i - 4]++;
                    keyState[i - 4] %= 6;
                    KeyModels[i - 4].material = KeyMaterials[keyState[i - 4]];
                    if (colorblindMode == true) KeyTexts[i - 4].text = colorblindLetters[keyState[i - 4]];
                }

                if(i % 5 != 0) { // Left
                    keyState[i - 1]++;
                    keyState[i - 1] %= 6;
                    KeyModels[i - 1].material = KeyMaterials[keyState[i - 1]];
                    if (colorblindMode == true) KeyTexts[i - 1].text = colorblindLetters[keyState[i - 1]];
                }

                if (i % 5 != 4) { // Right
                    keyState[i + 1]++;
                    keyState[i + 1] %= 6;
                    KeyModels[i + 1].material = KeyMaterials[keyState[i + 1]];
                    if (colorblindMode == true) KeyTexts[i + 1].text = colorblindLetters[keyState[i + 1]];
                }

                if (i < 20 && i % 5 != 0) { // Down-left
                    keyState[i + 4]++;
                    keyState[i + 4] %= 6;
                    KeyModels[i + 4].material = KeyMaterials[keyState[i + 4]];
                    if (colorblindMode == true) KeyTexts[i + 4].text = colorblindLetters[keyState[i + 4]];
                }

                if (i < 20) { // Down
                    keyState[i + 5]++;
                    keyState[i + 5] %= 6;
                    KeyModels[i + 5].material = KeyMaterials[keyState[i + 5]];
                    if (colorblindMode == true) KeyTexts[i + 5].text = colorblindLetters[keyState[i + 5]];
                }

                if (i < 20 && i % 5 != 4) { // Down-right
                    keyState[i + 6]++;
                    keyState[i + 6] %= 6;
                    KeyModels[i + 6].material = KeyMaterials[keyState[i + 6]];
                    if (colorblindMode == true) KeyTexts[i + 6].text = colorblindLetters[keyState[i + 6]];
                }

                break;

                case 5: // Magenta

                // Top-left corner
                keyState[0]++;
                keyState[0] %= 6;
                KeyModels[0].material = KeyMaterials[keyState[0]];
                if (colorblindMode == true) KeyTexts[0].text = colorblindLetters[keyState[0]];

                // Top-right corner
                keyState[4]++;
                keyState[4] %= 6;
                KeyModels[4].material = KeyMaterials[keyState[4]];
                if (colorblindMode == true) KeyTexts[4].text = colorblindLetters[keyState[4]];

                // Bottom-left corner
                keyState[20]++;
                keyState[20] %= 6;
                KeyModels[20].material = KeyMaterials[keyState[20]];
                if (colorblindMode == true) KeyTexts[20].text = colorblindLetters[keyState[20]];

                // Bottom-right corner
                keyState[24]++;
                keyState[24] %= 6;
                KeyModels[24].material = KeyMaterials[keyState[24]];
                if (colorblindMode == true) KeyTexts[24].text = colorblindLetters[keyState[24]];

                break;

                default:

                // Same position
                keyState[i]++;
                keyState[i] %= 6;
                KeyModels[i].material = KeyMaterials[keyState[i]];
                if (colorblindMode == true) KeyTexts[i].text = colorblindLetters[keyState[i]];

                break;
                }


                // Initial check for numbers
                for (int j = 0; j < colorCount.Length; j++)
                    colorCount[j] = 0;

                for (int j = 0; j < keyState.Length; j++) {
                    colorCount[keyState[j]]++;
                }

                presentColors = 0;

                for (int j = 0; j < colorCount.Length; j++) {
                    if (colorCount[j] > 0) presentColors++;
                }

                positionChecked = false;
            }

            // Stage 3
            else if (stage == 3) {
                // Delete key
                if (i == 20) {
                    if (enteredLength == 0)
                        Audio.PlaySoundAtTransform("TCS_Limit", transform);

                    else {
                        enteredLength--;
                        if (enteredLength == 0)
                            enteredText = "";

                        else
                            enteredText = enteredText.Substring(0, enteredLength);

                        ScreenText.text = enteredText;
                    }
                }

                // Submit key
                else if (i == 24)
                    Submit();

                else if (enteredLength == 10)
                    Audio.PlaySoundAtTransform("TCS_Limit", transform);

                else {
                    enteredLength++;
                    enteredText += keyLetters[i];
                    ScreenText.text = enteredText;
                }
            }
        }
    }


    // Stage 3 Submit
    private void Submit() {
        Debug.LogFormat("[Three Cryptic Steps #{0}] You submitted: {1}", moduleId, enteredText);

        if (Bomb.GetSolvedModuleNames().Count() != 0)
            lastSolvedModule = Bomb.GetSolvedModuleNames().Last();

        Debug.LogFormat("[Three Cryptic Steps #{0}] The solved module that comes last in alphabetical order is: {1}", moduleId, lastSolvedModule);

        string name = FixModuleName(lastSolvedModule);

        if (name.Length != 0)
            submitText = GetSubmitText(name);

        else
            submitText = "";

        Debug.LogFormat("[Three Cryptic Steps #{0}] The expected answer is: {1}", moduleId, submitText);

        // Answer checking
        if (enteredText == submitText) {
            Debug.LogFormat("[Three Cryptic Steps #{0}] Module solved! Congratulations!", moduleId);
            Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.CorrectChime, transform);
            GetComponent<KMBombModule>().HandlePass();
            stage = 4;
        }

        // Specific words
        else if (enteredText == "PASSWORD" && enteredPassword == false)
            StartCoroutine(StageThreePassword());

        else if (enteredText == "OUTBURST" && enteredOutburst == false)
            StartCoroutine(StageThreeOutburst());

        else if (enteredText == "LLABTEKSAB" && enteredBasketball == false)
            StartCoroutine(StageThreeBasketball());

        else if (enteredText == "SAMWICH" && enteredSandwhich == false) {
            enteredSandwhich = true;
            Audio.PlaySoundAtTransform("TCS_Samwich", transform);
            Debug.LogFormat("[Three Cryptic Steps #{0}] You found the secret password!", moduleId);
            ScreenText.text = "";
            enteredText = "";
            enteredLength = 0;
        }

        // Incorrect
        else {
            moduleStrikes++;
            StartCoroutine(StageThreeStrike());
        }

        if (stage == 4) {
            ScreenText.color = ScreenColors[2];
            ScreenText.text = "Solved!";
        }
    }

    // Modifies the module name
    private string FixModuleName(string name) {
        string str = "";
        char[] fixedName = name.ToCharArray();

        for (int i = 0; i < fixedName.Length && str.Length < 10; i++) {
            for (int j = 0; j < capitalLetters.Length; j++) {
                if (fixedName[i] == capitalLetters[j] || fixedName[i] == lowercaseLetters[j]) {
                    str += capitalLetters[j];
                    break;
                }
            }
        }

        return str;
    }

    // Creates the submit text
    private string GetSubmitText(string name) {
        string str = "";
        char[] fixedName = name.ToCharArray();

        for (int i = 0; i < fixedName.Length; i++) {
            for (int j = 0; j < capitalLetters.Length; j++) {
                if (fixedName[i] == capitalLetters[j]) {
                    str += majorLetters[(j + 1) * serialDigitSum % 23];
                    break;
                }
            }
        }

        string newStr = "";
        char[] fixedStr = str.ToCharArray();

        for (int i = fixedStr.Length - 1; i >= 0; i--)
            newStr += fixedStr[i];

        return newStr;
    }


    // Stage 3 Entered "PASSWORD"
    private IEnumerator StageThreePassword() {
        canPress = false;
        enteredPassword = true;
        Audio.PlaySoundAtTransform("TCS_Alert", transform);

        Debug.LogFormat("[Three Cryptic Steps #{0}] Enter this display for Skyeward's Unfair Cipher:", moduleId);
        Debug.LogFormat("[Three Cryptic Steps #{0}] QTRDLEGZ", moduleId);
        Debug.LogFormat("[Three Cryptic Steps #{0}] YYWWMMCC", moduleId);
        Debug.LogFormat("[Three Cryptic Steps #{0}] 57160152", moduleId);

        yield return new WaitForSeconds(0.04f);
        ScreenText.color = ScreenColors[1];
        ScreenText.text = "LFA";
        yield return new WaitForSeconds(0.25f);
        ScreenText.text = "";
        yield return new WaitForSeconds(0.15f);
        ScreenText.text = "LFA";
        yield return new WaitForSeconds(0.25f);
        ScreenText.text = "";
        yield return new WaitForSeconds(0.15f);
        ScreenText.text = "LFA";
        yield return new WaitForSeconds(0.8f);
        ScreenText.text = "";
        ScreenText.color = ScreenColors[8];
        yield return new WaitForSeconds(0.15f);
        ScreenText.text = "";
        enteredText = "";
        enteredLength = 0;
        canPress = true;
    }

    // Stage 3 Entered "OUTBURST"
    private IEnumerator StageThreeOutburst() {
        canPress = false;
        enteredOutburst = true;
        ScreenText.color = ScreenColors[1];
        ScreenText.text = "Get Ready";
        Debug.LogFormat("[Three Cryptic Steps #{0}] I hope you have good listening ears!", moduleId);
        yield return new WaitForSeconds(2.0f);
        Audio.PlaySoundAtTransform("TCS_Morse", transform);
        yield return new WaitForSeconds(110.0f);
        ScreenText.text = "";
        enteredText = "";
        enteredLength = 0;
        canPress = true;
    }

    // Stage 3 Entered "LLABTEKSAB"
    private IEnumerator StageThreeBasketball() {
        canPress = false;
        enteredBasketball = true;
        ScreenText.color = ScreenColors[1];
        ScreenText.text = "GLMIZIE";
        Debug.LogFormat("[Three Cryptic Steps #{0}] Don't get bamboozled by this!", moduleId);
        Audio.PlaySoundAtTransform("TCS_BA", transform);
        Audio.PlaySoundAtTransform("TCS_Sound1", transform);
        yield return new WaitForSeconds(1.0f);
        ScreenText.color = ScreenColors[4];
        ScreenText.text = "KRXVJ";
        Audio.PlaySoundAtTransform("TCS_Sound1", transform);
        yield return new WaitForSeconds(1.0f);
        ScreenText.color = ScreenColors[5];
        ScreenText.text = "ESCPP";
        Audio.PlaySoundAtTransform("TCS_Sound1", transform);
        yield return new WaitForSeconds(1.0f);
        ScreenText.color = ScreenColors[2];
        ScreenText.text = "CD";
        Audio.PlaySoundAtTransform("TCS_Sound1", transform);
        yield return new WaitForSeconds(1.0f);
        ScreenText.color = ScreenColors[8];
        ScreenText.text = "TEEBHGOBKMN";
        Audio.PlaySoundAtTransform("TCS_Sound1", transform);
        yield return new WaitForSeconds(1.0f);
        ScreenText.color = ScreenColors[3];
        ScreenText.text = "BKQE";
        Audio.PlaySoundAtTransform("TCS_Sound1", transform);
        yield return new WaitForSeconds(1.0f);
        ScreenText.color = ScreenColors[0];
        ScreenText.text = "AOKHR";
        Audio.PlaySoundAtTransform("TCS_Sound1", transform);
        yield return new WaitForSeconds(1.0f);
        ScreenText.color = ScreenColors[7];
        ScreenText.text = "RFZBQHY";
        Audio.PlaySoundAtTransform("TCS_Sound1", transform);
        yield return new WaitForSeconds(1.0f);
        Audio.PlaySoundAtTransform("TCS_Sound1", transform);
        ScreenText.color = ScreenColors[8];
        ScreenText.text = "";
        enteredText = "";
        enteredLength = 0;
        canPress = true;
    }

    // Stage 3 Strike
    private IEnumerator StageThreeStrike() {
        canPress = false;
        ScreenText.text = "That's";
        ScreenText.color = ScreenColors[4];
        Audio.PlaySoundAtTransform("TCS_Sound1", transform);

        yield return new WaitForSeconds(0.4f);
        Audio.PlaySoundAtTransform("TCS_Sound1", transform);
        Debug.LogFormat("[Three Cryptic Steps #{0}] That's Wrong! Strike received!", moduleId);
        GetComponent<KMBombModule>().HandleStrike();
        ScreenText.text = "WRONG!";
        ScreenText.color = ScreenColors[0];

        if (moduleStrikes == 3) {
            yield return new WaitForSeconds(0.7f);
            ScreenText.text = "LFA";
            ScreenText.color = ScreenColors[1];

            Debug.LogFormat("[Three Cryptic Steps #{0}] Enter this display for Skyeward's Unfair Cipher:", moduleId);
            Debug.LogFormat("[Three Cryptic Steps #{0}] QTRDLEGZ", moduleId);
            Debug.LogFormat("[Three Cryptic Steps #{0}] YYWWMMCC", moduleId);
            Debug.LogFormat("[Three Cryptic Steps #{0}] 57160152", moduleId);
        }

        yield return new WaitForSeconds(0.3f);
        ScreenText.color = ScreenColors[0];
        ScreenText.text = "";
        enteredText = "";
        enteredLength = 0;
        ScreenText.color = ScreenColors[8];
        canPress = true;
    }


    // Stage 1 Advance
    private IEnumerator StageOneAdvance() {
        ScreenText.text = "Let's";
        ScreenText.color = ScreenColors[5];
        Audio.PlaySoundAtTransform("TCS_Sound1", transform);

        yield return new WaitForSeconds(0.15f);
        ScreenText.text = "See,";
        ScreenText.color = ScreenColors[4];
        Audio.PlaySoundAtTransform("TCS_Sound2", transform);

        yield return new WaitForSeconds(0.3f);
        ScreenText.text = "That";
        ScreenText.color = ScreenColors[3];
        Audio.PlaySoundAtTransform("TCS_Sound1", transform);

        yield return new WaitForSeconds(0.3f);
        ScreenText.text = "Is";
        ScreenText.color = ScreenColors[8];
        Audio.PlaySoundAtTransform("TCS_Sound2", transform);

        yield return new WaitForSeconds(0.15f);
        ScreenText.text = "CORRECT!";
        ScreenText.color = ScreenColors[2];
        Audio.PlaySoundAtTransform("TCS_Sound1", transform);
        Debug.LogFormat("[Three Cryptic Steps #{0}] Step one complete!", moduleId);

        yield return new WaitForSeconds(0.3f);
        stage = 2;
        moduleStrikes = 0;
        ScreenText.text = "Step Two";
        ScreenText.color = ScreenColors[8];

        Debug.LogFormat("[Three Cryptic Steps #{0}] The easy numbers allowed to form are: {1}", moduleId, GetEasyNumbers());
        Debug.LogFormat("[Three Cryptic Steps #{0}] The hard numbers allowed to form are: {1}", moduleId, GetHardNumbers());

        Audio.PlaySoundAtTransform("TCS_Group", transform);
        for (int i = 0; i < group1.Length; i++) {
            int rand = UnityEngine.Random.Range(0, 6);
            KeyModels[group1[i]].material = KeyMaterials[rand];
            keyState[group1[i]] = rand;
            if (colorblindMode == true) KeyTexts[group1[i]].text = colorblindLetters[keyState[group1[i]]];
        }

        yield return new WaitForSeconds(0.3f);
        Audio.PlaySoundAtTransform("TCS_Group", transform);
        for (int i = 0; i < group2.Length; i++) {
            int rand = UnityEngine.Random.Range(0, 6);
            KeyModels[group2[i]].material = KeyMaterials[rand];
            keyState[group2[i]] = rand;
            if (colorblindMode == true) KeyTexts[group2[i]].text = colorblindLetters[keyState[group2[i]]];
        }

        yield return new WaitForSeconds(0.3f);
        Audio.PlaySoundAtTransform("TCS_Group", transform);
        for (int i = 0; i < group3.Length; i++) {
            int rand = UnityEngine.Random.Range(0, 6);
            KeyModels[group3[i]].material = KeyMaterials[rand];
            keyState[group3[i]] = rand;
            if (colorblindMode == true) KeyTexts[group3[i]].text = colorblindLetters[keyState[group3[i]]];
        }

        yield return new WaitForSeconds(0.3f);
        Audio.PlaySoundAtTransform("TCS_Group", transform);
        for (int i = 0; i < group4.Length; i++) {
            int rand = UnityEngine.Random.Range(0, 6);
            KeyModels[group4[i]].material = KeyMaterials[rand];
            keyState[group4[i]] = rand;
            if (colorblindMode == true) KeyTexts[group4[i]].text = colorblindLetters[keyState[group4[i]]];
        }

        yield return new WaitForSeconds(0.3f);
        Audio.PlaySoundAtTransform("TCS_Group", transform);
        for (int i = 0; i < group5.Length; i++) {
            int rand = UnityEngine.Random.Range(0, 6);
            KeyModels[group5[i]].material = KeyMaterials[rand];
            keyState[group5[i]] = rand;
            if (colorblindMode == true) KeyTexts[group5[i]].text = colorblindLetters[keyState[group5[i]]];
        }

        yield return new WaitForSeconds(0.3f);
        Audio.PlaySoundAtTransform("TCS_Group", transform);
        for (int i = 0; i < group6.Length; i++) {
            int rand = UnityEngine.Random.Range(0, 6);
            KeyModels[group6[i]].material = KeyMaterials[rand];
            keyState[group6[i]] = rand;
            if (colorblindMode == true) KeyTexts[group6[i]].text = colorblindLetters[keyState[group6[i]]];
        }

        yield return new WaitForSeconds(0.3f);
        Audio.PlaySoundAtTransform("TCS_Group", transform);
        for (int i = 0; i < group7.Length; i++) {
            int rand = UnityEngine.Random.Range(0, 6);
            KeyModels[group7[i]].material = KeyMaterials[rand];
            keyState[group7[i]] = rand;
            if (colorblindMode == true) KeyTexts[group7[i]].text = colorblindLetters[keyState[group7[i]]];
        }

        yield return new WaitForSeconds(0.3f);
        Audio.PlaySoundAtTransform("TCS_Group", transform);
        for (int i = 0; i < group8.Length; i++) {
            int rand = UnityEngine.Random.Range(0, 6);
            KeyModels[group8[i]].material = KeyMaterials[rand];
            keyState[group8[i]] = rand;
            if (colorblindMode == true) KeyTexts[group8[i]].text = colorblindLetters[keyState[group8[i]]];
        }

        yield return new WaitForSeconds(0.3f);
        Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
        ScreenText.text = "";
        for (int i = 0; i < NumberTexts.Length; i++)
            NumberTexts[i].text = i.ToString();

        canPress = true;
    }

    // Stage 1 Strike
    private IEnumerator StageOneStrike() {
        ScreenText.text = "Let's";
        ScreenText.color = ScreenColors[5];
        Audio.PlaySoundAtTransform("TCS_Sound1", transform);

        yield return new WaitForSeconds(0.15f);
        ScreenText.text = "See,";
        ScreenText.color = ScreenColors[4];
        Audio.PlaySoundAtTransform("TCS_Sound2", transform);

        yield return new WaitForSeconds(0.3f);
        ScreenText.text = "That";
        ScreenText.color = ScreenColors[3];
        Audio.PlaySoundAtTransform("TCS_Sound1", transform);

        yield return new WaitForSeconds(0.3f);
        ScreenText.text = "Is";
        ScreenText.color = ScreenColors[8];
        Audio.PlaySoundAtTransform("TCS_Sound2", transform);

        yield return new WaitForSeconds(0.15f);
        ScreenText.text = "WRONG!";
        ScreenText.color = ScreenColors[0];
        Audio.PlaySoundAtTransform("TCS_Sound1", transform);
        Debug.LogFormat("[Three Cryptic Steps #{0}] Invalid button press for step one! Strike received!", moduleId);
        GetComponent<KMBombModule>().HandleStrike();
        moduleStrikes++;

        if (moduleStrikes == 3) {
            yield return new WaitForSeconds(0.7f);
            ScreenText.text = "%10'-4\"!32'";
            ScreenText.color = ScreenColors[1];
        }

        yield return new WaitForSeconds(0.3f);
        ScreenText.text = "Step One";
        ScreenText.color = ScreenColors[8];
        canPress = true;
    }


    // Number and Module Checks
    private void Update() {
        if (stage == 2 && positionChecked == false) {
            positionChecked = true;
            canPress = false;

            if (presentColors > 1) {
                int validNumber = -1;
                for (int i = 0; i < numberFound.Length; i++) {
                    if (numberFound[i] == false && validNumber == -1) {
                        bool valid = true;

                        switch (i) {
                        case 1:
                        for (int j = 1; j < number1.Length; j++) {
                            if (keyState[number1[0]] != keyState[number1[j]]) {
                                valid = false;
                                break;
                            }
                        }

                        for (int j = 0; j < notNumber1.Length; j++) {
                            if (keyState[notNumber1[j]] == keyState[number1[0]]) {
                                valid = false;
                                break;
                            }
                        }

                        break;

                        case 2:
                        for (int j = 1; j < number2.Length; j++) {
                            if (keyState[number2[0]] != keyState[number2[j]]) {
                                valid = false;
                                break;
                            }
                        }

                        for (int j = 0; j < notNumber2.Length; j++) {
                            if (keyState[notNumber2[j]] == keyState[number2[0]]) {
                                valid = false;
                                break;
                            }
                        }

                        break;

                        case 3:
                        for (int j = 1; j < number3.Length; j++) {
                            if (keyState[number3[0]] != keyState[number3[j]]) {
                                valid = false;
                                break;
                            }
                        }

                        for (int j = 0; j < notNumber3.Length; j++) {
                            if (keyState[notNumber3[j]] == keyState[number3[0]]) {
                                valid = false;
                                break;
                            }
                        }

                        break;

                        case 4:
                        for (int j = 1; j < number4.Length; j++) {
                            if (keyState[number4[0]] != keyState[number4[j]]) {
                                valid = false;
                                break;
                            }
                        }

                        for (int j = 0; j < notNumber4.Length; j++) {
                            if (keyState[notNumber4[j]] == keyState[number4[0]]) {
                                valid = false;
                                break;
                            }
                        }

                        break;

                        case 5:
                        for (int j = 1; j < number5.Length; j++) {
                            if (keyState[number5[0]] != keyState[number5[j]]) {
                                valid = false;
                                break;
                            }
                        }

                        for (int j = 0; j < notNumber5.Length; j++) {
                            if (keyState[notNumber5[j]] == keyState[number5[0]]) {
                                valid = false;
                                break;
                            }
                        }

                        break;

                        case 6:
                        for (int j = 1; j < number6.Length; j++) {
                            if (keyState[number6[0]] != keyState[number6[j]]) {
                                valid = false;
                                break;
                            }
                        }

                        for (int j = 0; j < notNumber6.Length; j++) {
                            if (keyState[notNumber6[j]] == keyState[number6[0]]) {
                                valid = false;
                                break;
                            }
                        }

                        break;

                        case 7:
                        for (int j = 1; j < number7.Length; j++) {
                            if (keyState[number7[0]] != keyState[number7[j]]) {
                                valid = false;
                                break;
                            }
                        }

                        for (int j = 0; j < notNumber7.Length; j++) {
                            if (keyState[notNumber7[j]] == keyState[number7[0]]) {
                                valid = false;
                                break;
                            }
                        }

                        break;

                        case 8:
                        for (int j = 1; j < number8.Length; j++) {
                            if (keyState[number8[0]] != keyState[number8[j]]) {
                                valid = false;
                                break;
                            }
                        }

                        for (int j = 0; j < notNumber8.Length; j++) {
                            if (keyState[notNumber8[j]] == keyState[number8[0]]) {
                                valid = false;
                                break;
                            }
                        }

                        break;

                        case 9:
                        for (int j = 1; j < number9.Length; j++) {
                            if (keyState[number9[0]] != keyState[number9[j]]) {
                                valid = false;
                                break;
                            }
                        }

                        for (int j = 0; j < notNumber9.Length; j++) {
                            if (keyState[notNumber9[j]] == keyState[number9[0]]) {
                                valid = false;
                                break;
                            }
                        }

                        break;

                        default:
                        for (int j = 1; j < number0.Length; j++) {
                            if (keyState[number0[0]] != keyState[number0[j]]) {
                                valid = false;
                                break;
                            }
                        }

                        for (int j = 0; j < notNumber0.Length; j++) {
                            if (keyState[notNumber0[j]] == keyState[number0[0]]) {
                                valid = false;
                                break;
                            }
                        }

                        break;
                        }

                        if (valid == true) validNumber = i;
                    }
                }

                bool willStrike = false;

                if (validNumber > -1 && numberFound[validNumber] == false) {
                    Debug.LogFormat("[Three Cryptic Steps #{0}] You formed the number {1}.", moduleId, validNumber);
                    allowTenMinCheck = false;
                    numberFound[validNumber] = true;

                    // Invalid number
                    if (validSerialNumbers[validNumber] == false) {
                        willStrike = true;
                        moduleStrikes++;
                        numberColors[validNumber] = 7;
                        NumberTexts[validNumber].color = ScreenColors[7];
                        Debug.LogFormat("[Three Cryptic Steps #{0}] That number is invalid! Strike received!", moduleId);
                    }

                    // Valid number
                    else {
                        switch (validNumber) {
                        case 1: numberColors[validNumber] = keyState[number1[0]]; break;
                        case 2: numberColors[validNumber] = keyState[number2[0]]; break;
                        case 3: numberColors[validNumber] = keyState[number3[0]]; break;
                        case 4: numberColors[validNumber] = keyState[number4[0]]; break;
                        case 5: numberColors[validNumber] = keyState[number5[0]]; break;
                        case 6: numberColors[validNumber] = keyState[number6[0]]; break;
                        case 7: numberColors[validNumber] = keyState[number7[0]]; break;
                        case 8: numberColors[validNumber] = keyState[number8[0]]; break;
                        case 9: numberColors[validNumber] = keyState[number9[0]]; break;
                        default: numberColors[validNumber] = keyState[number0[0]]; break;
                        }

                        NumberTexts[validNumber].color = ScreenColors[numberColors[validNumber]];
                        validNumbersFound++;
                        Audio.PlaySoundAtTransform("TCS_NumberFound", transform);

                        Debug.LogFormat("[Three Cryptic Steps #{0}] That number is valid!", moduleId);

                        // Hard number found
                        for (int i = 0; i < hardNumbers.Length; i++) {
                            if (validNumber == hardNumbers[i]) {
                                hardNumberFound = true;
                                Debug.LogFormat("[Three Cryptic Steps #{0}] A hard number has been formed! Congratulations!", moduleId);
                                break;
                            }
                        }

                        // If the color has already been used
                        for (int i = 0; i < numberColors.Length; i++) {
                            if (i != validNumber && numberColors[i] == numberColors[validNumber]) {
                                willStrike = true;
                                moduleStrikes++;
                                Debug.LogFormat("[Three Cryptic Steps #{0}] That color has already been stored for another number. Strike received!", moduleId);
                                break;
                            }
                        }
                    }

                    // Strikes
                    if (willStrike == true && validNumbersFound != validSerialNumberCount && hardNumberFound == false) {
                        willStrike = false;
                        GetComponent<KMBombModule>().HandleStrike();

                        if (moduleStrikes == 3)
                            StartCoroutine(StageTwoStrike());

                        else
                            canPress = true;
                    }

                    // Strikes then advances
                    else if (willStrike == true) {
                        willStrike = false;
                        GetComponent<KMBombModule>().HandleStrike();
                        StartCoroutine(StageTwoFaultyAdvance());
                    }

                    // Advances
                    else if (validNumbersFound == validSerialNumberCount || hardNumberFound == true)
                        StartCoroutine(StageTwoAdvance());

                    else
                        canPress = true;
                }

                else
                    canPress = true;
            }
            
            else 
                canPress = true;
        }

        // Shows the video with the secret password
        else if (stage == 3 && isVirtual == true && shownPassword == false && 
            Bomb.GetSolvedModuleNames().Count() >= solveMarker)
                StartCoroutine(ShowPasswordVideo());
    }

    // Shows Password Video
    private IEnumerator ShowPasswordVideo() {
        canPress = false;
        shownPassword = true;
        yield return new WaitForSeconds(1.5f);
        Debug.LogFormat("[Three Cryptic Steps #{0}] The secret of the Virtuallion has revealed itself!", moduleId);
        Audio.PlaySoundAtTransform("TCS_Alert", transform);
        yield return new WaitForSeconds(0.04f);
        ScreenText.color = ScreenColors[1];
        ScreenText.text = "zMeIicZa8dg";
        yield return new WaitForSeconds(0.25f);
        ScreenText.text = "";
        yield return new WaitForSeconds(0.15f);
        ScreenText.text = "zMeIicZa8dg";
        yield return new WaitForSeconds(0.25f);
        ScreenText.text = "";
        yield return new WaitForSeconds(0.15f);
        ScreenText.text = "zMeIicZa8dg";
        yield return new WaitForSeconds(5.0f);
        ScreenText.text = "";
        ScreenText.color = ScreenColors[8];
        yield return new WaitForSeconds(0.15f);
        ScreenText.text = "";
        enteredText = "";
        enteredLength = 0;
        canPress = true;
    }


    // Stage 2 Strike
    private IEnumerator StageTwoStrike() {
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < NumberTexts.Length; i++)
            NumberTexts[i].text = "";

        ScreenText.color = ScreenColors[1];
        ScreenText.text = Bomb.GetSerialNumber();

        yield return new WaitForSeconds(0.3f);
        ScreenText.text = "";
        ScreenText.color = ScreenColors[8];
        for (int i = 0; i < NumberTexts.Length; i++)
            NumberTexts[i].text = i.ToString();

        canPress = true;
    }

    // Stage 2 Advance
    private IEnumerator StageTwoAdvance() {
        Debug.LogFormat("[Three Cryptic Steps #{0}] Step two complete!", moduleId);
        yield return new WaitForSeconds(0.6f);
        for (int i = 0; i < NumberTexts.Length; i++)
            NumberTexts[i].text = "";

        ScreenText.text = "Good";
        ScreenText.color = ScreenColors[5];
        Audio.PlaySoundAtTransform("TCS_Sound1", transform);

        yield return new WaitForSeconds(0.15f);
        ScreenText.text = "Job,";
        ScreenText.color = ScreenColors[4];
        Audio.PlaySoundAtTransform("TCS_Sound2", transform);

        yield return new WaitForSeconds(0.3f);
        ScreenText.text = "You";
        ScreenText.color = ScreenColors[3];
        Audio.PlaySoundAtTransform("TCS_Sound1", transform);

        yield return new WaitForSeconds(0.3f);
        ScreenText.text = "Did";
        ScreenText.color = ScreenColors[8];
        Audio.PlaySoundAtTransform("TCS_Sound2", transform);

        yield return new WaitForSeconds(0.15f);
        ScreenText.text = "It!";
        ScreenText.color = ScreenColors[2];
        Audio.PlaySoundAtTransform("TCS_Sound1", transform);

        StartCoroutine(StageThreeStart());
    }

    // Stage 2 Strike + Advance
    private IEnumerator StageTwoFaultyAdvance() {
        yield return new WaitForSeconds(1.2f);
        for (int i = 0; i < NumberTexts.Length; i++)
            NumberTexts[i].text = "";

        Debug.LogFormat("[Three Cryptic Steps #{0}] Step two complete! Even if it wasn't the way you wanted it.", moduleId);
        ScreenText.text = "Well,";
        ScreenText.color = ScreenColors[5];
        Audio.PlaySoundAtTransform("TCS_Sound1", transform);

        yield return new WaitForSeconds(0.15f);
        ScreenText.text = "Let's";
        ScreenText.color = ScreenColors[4];
        Audio.PlaySoundAtTransform("TCS_Sound2", transform);

        yield return new WaitForSeconds(0.3f);
        ScreenText.text = "Move";
        ScreenText.color = ScreenColors[3];
        Audio.PlaySoundAtTransform("TCS_Sound1", transform);

        yield return new WaitForSeconds(0.3f);
        ScreenText.text = "On";
        ScreenText.color = ScreenColors[8];
        Audio.PlaySoundAtTransform("TCS_Sound2", transform);

        yield return new WaitForSeconds(0.15f);
        ScreenText.text = "Anyway";
        ScreenText.color = ScreenColors[1];
        Audio.PlaySoundAtTransform("TCS_Sound1", transform);

        StartCoroutine(StageThreeStart());
    }

    // Wait 10 minutes
    private IEnumerator WaitTenMinutes() {
        yield return new WaitForSeconds(600.0f);
        if (allowTenMinCheck == true) {
            canPress = false;
            for (int i = 0; i < NumberTexts.Length; i++)
                NumberTexts[i].text = "";

            Audio.PlaySoundAtTransform("TCS_Alert", transform);
            yield return new WaitForSeconds(0.04f);
            ScreenText.color = ScreenColors[1];
            ScreenText.text = "RS5";
            yield return new WaitForSeconds(0.25f);
            ScreenText.text = "";
            yield return new WaitForSeconds(0.15f);
            ScreenText.text = "RS5";
            yield return new WaitForSeconds(0.25f);
            ScreenText.text = "";
            yield return new WaitForSeconds(0.15f);
            ScreenText.text = "RS5";
            yield return new WaitForSeconds(0.8f);
            ScreenText.text = "";
            ScreenText.color = ScreenColors[8];
            yield return new WaitForSeconds(0.15f);
            for (int i = 0; i < NumberTexts.Length; i++)
                NumberTexts[i].text = i.ToString();

            canPress = true;
        }
    }

    
    // Stage 3 Start
    private IEnumerator StageThreeStart() {
        yield return new WaitForSeconds(0.3f);
        ScreenText.color = ScreenColors[8];
        moduleStrikes = 0;
        ScreenText.text = "Step Three";

        Audio.PlaySoundAtTransform("TCS_Group", transform);
        for (int i = 0; i < group1.Length; i++) {
            KeyModels[group1[i]].material = KeyMaterials[8];
            KeyTexts[group1[i]].text = keyLetters[group1[i]];
        }

        yield return new WaitForSeconds(0.3f);
        Audio.PlaySoundAtTransform("TCS_Group", transform);
        for (int i = 0; i < group2.Length; i++) {
            KeyModels[group2[i]].material = KeyMaterials[8];
            KeyTexts[group2[i]].text = keyLetters[group2[i]];
        }

        yield return new WaitForSeconds(0.3f);
        Audio.PlaySoundAtTransform("TCS_Group", transform);
        for (int i = 0; i < group3.Length; i++) {
            KeyModels[group3[i]].material = KeyMaterials[8];
            KeyTexts[group3[i]].text = keyLetters[group3[i]];
        }

        yield return new WaitForSeconds(0.3f);
        Audio.PlaySoundAtTransform("TCS_Group", transform);
        for (int i = 0; i < group4.Length; i++) {
            KeyModels[group4[i]].material = KeyMaterials[8];
            KeyTexts[group4[i]].text = keyLetters[group4[i]];
        }

        yield return new WaitForSeconds(0.3f);
        Audio.PlaySoundAtTransform("TCS_Group", transform);
        for (int i = 0; i < group5.Length; i++) {
            KeyModels[group5[i]].material = KeyMaterials[8];
            KeyTexts[group5[i]].text = keyLetters[group5[i]];

            if (group5[i] == 24)
                KeyModels[group5[i]].material = KeyMaterials[2];
        }

        yield return new WaitForSeconds(0.3f);
        Audio.PlaySoundAtTransform("TCS_Group", transform);
        for (int i = 0; i < group6.Length; i++) {
            KeyModels[group6[i]].material = KeyMaterials[8];
            KeyTexts[group6[i]].text = keyLetters[group6[i]];
        }

        yield return new WaitForSeconds(0.3f);
        Audio.PlaySoundAtTransform("TCS_Group", transform);
        for (int i = 0; i < group7.Length; i++) {
            KeyModels[group7[i]].material = KeyMaterials[8];
            KeyTexts[group7[i]].text = keyLetters[group7[i]];

            if (group7[i] == 20)
                KeyModels[group7[i]].material = KeyMaterials[0];
        }

        yield return new WaitForSeconds(0.3f);
        Audio.PlaySoundAtTransform("TCS_Group", transform);

        yield return new WaitForSeconds(0.3f);
        Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
        stage = 3;
        canPress = true;
    }


    // Gets the easy numbers for logging
    private string GetEasyNumbers() {
        string msg = "";

        for (int i = 0; i < easyNumbers.Length; i++) {
            if (validSerialNumbers[easyNumbers[i]] == true) msg += easyNumbers[i].ToString() + " ";
        }

        return msg;
    }

    // Gets the hard numbers for logging
    private string GetHardNumbers() {
        string msg = "";

        for (int i = 0; i < hardNumbers.Length; i++) {
            if (validSerialNumbers[hardNumbers[i]] == true) msg += hardNumbers[i].ToString() + " ";
        }

        return msg;
    }
}