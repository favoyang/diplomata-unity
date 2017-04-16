﻿using System;
using UnityEngine;
using UnityEditor;

namespace DiplomataEditor {

    public class DGUI {

        public const byte MARGIN = 10;
        public const byte PADDING = 5;

        public const byte BUTTON_HEIGHT_SMALL = 20;
        public const byte BUTTON_HEIGHT = 30;
        public const byte BUTTON_HEIGHT_BIG = 40;

        public const int WINDOW_MIN_WIDTH = 400;

        public static Rect noClipRect;
        public static Color BGColor = new Color(0.9764f, 0.9764f, 0.9764f);
        public static Color proBGColor = new Color(0.2196f, 0.2196f, 0.2196f);
        public static bool hasSlider;
        public static Texture2D transparent = UniformColorTexture(1, 1, new Color(0,0,0,0));
        
        public static void Vertical(Action callback) {
            GUILayout.BeginVertical();
            callback();
            GUILayout.EndVertical();
        }

        public static void Vertical(Action callback, int height) {
            GUILayout.BeginVertical(GUILayout.Height(height));
            callback();
            GUILayout.EndVertical();
        }

        public static void Vertical(Action callback, params GUILayoutOption[] options) {
            GUILayout.BeginVertical(options);
            callback();
            GUILayout.EndVertical();
        }

        public static void Horizontal(Action callback) {
            GUILayout.BeginHorizontal();
            callback();
            GUILayout.EndHorizontal();
        }

        public static void Horizontal(Action callback, int width) {
            GUILayout.BeginHorizontal(GUILayout.Width(width));
            callback();
            GUILayout.EndHorizontal();
        }

        public static void Horizontal(Action callback, params GUILayoutOption[] options) {
            GUILayout.BeginHorizontal(options);
            callback();
            GUILayout.EndHorizontal();
        }

        public static void WindowWrap(Action callback) {
            Horizontal(() => {
                GUILayout.Space(MARGIN);

                Vertical(() => {
                    GUILayout.Space(MARGIN);

                    callback();

                    GUILayout.Space(MARGIN);
                });

                GUILayout.Space(MARGIN);
            });
        }

        public static Vector2 ScrollWrap(Action callback, Vector2 scrollPosInput, Rect clipRect, Rect contentRect) {
            Vector2 scrollPosOutput = new Vector2(0, 0);
            scrollPosOutput = scrollPosInput;
            
            scrollPosOutput = GUI.BeginScrollView(clipRect, scrollPosOutput, contentRect);

            Horizontal(() => {
                GUILayout.Space(MARGIN);

                Vertical(() => {
                    GUILayout.Space(MARGIN);

                    callback();

                    GUILayout.Space(MARGIN);
                });

                if (Screen.height - 22 < contentRect.height) {
                    hasSlider = true;
                    GUILayout.Space(MARGIN);
                }

                else {
                    hasSlider = false;
                }

                GUILayout.Space(MARGIN);
            });

            GUI.EndScrollView();

            return scrollPosOutput;
        }

        public static Vector2 ScrollWindow(Action callback, Vector2 scrollPosInput, float contentHeight) {
            noClipRect = new Rect(0, 0, Screen.width, Screen.height - 22);
            return ScrollWrap(callback, scrollPosInput, noClipRect, new Rect(0, 0, Screen.width - 15, contentHeight));
        }

        public static void Focus(Action callback, string name = "focus") {
            GUI.SetNextControlName(name);
            callback();
            EditorGUI.FocusTextInControl(name);
        }

        public static float Box(string text, float x, float y, float width, Color color, float extraHeight = 0, TextAnchor textAlign = TextAnchor.UpperCenter) {
            GUIStyle style = new GUIStyle();
            var content = new GUIContent(text);
            var height = style.CalcHeight(content, width);

            if (hasSlider) {
                width -= MARGIN;
            }
            
            style.alignment = textAlign;
            style.padding = new RectOffset(PADDING, PADDING, 2 * PADDING, PADDING);
            style.normal.textColor = Color.black;
            style.normal.background = transparent;

            if (color.r * color.g * color.b < 0.1f) {
                style.normal.textColor = Color.white;
            }

            Rect boxRect = new Rect(x, y, width, height + extraHeight);
            
            EditorGUI.DrawRect(new Rect(x - 1, y - 1, width + 2, height + extraHeight + 2), ColorSub(color, 0.5f, 0.5f, 0.5f));
            EditorGUI.DrawRect(boxRect, color);
            GUI.Box(boxRect, text, style);

            return height;
        }

        public static float Box(string text, float x, float y, float width, float extraHeight = 0, TextAnchor textAlign = TextAnchor.UpperCenter) {
            return Box(text, x, y, width, ResetColor(), extraHeight, textAlign);
        }
        
        public static float Box(string text, float x, float y, float width, Color color, TextAnchor textAlign = TextAnchor.UpperCenter) {
            return Box(text, x, y, width, color, 0, textAlign);
        }

        public static Color ColorAdd(Color color, float r, float g, float b, float a = 0) {
            Color newColor = new Color(0, 0, 0);
            newColor = color;
            newColor.r += r;
            newColor.g += g;
            newColor.b += b;
            newColor.a += a;
            return newColor;
        }

        public static Color ColorSub(Color color, float r, float g, float b, float a = 0) {
            Color newColor = new Color(0, 0, 0);
            newColor = color;
            newColor.r -= r;
            newColor.g -= g;
            newColor.b -= b;
            newColor.a -= a;
            return newColor;
        }

        public static Color ColorMul(Color color, float r, float g, float b, float a = 1) {
            Color newColor = new Color(0, 0, 0);
            newColor = color;
            newColor.r *= r;
            newColor.g *= g;
            newColor.b *= b;
            newColor.a *= a;
            return newColor;
        }

        public static Color ColorDiv(Color color, float r, float g, float b, float a = 1) {
            Color newColor = new Color(0, 0, 0);
            newColor = color;
            newColor.r /= r;
            newColor.g /= g;
            newColor.b /= b;
            newColor.a /= a;
            return newColor;
        }

        public static Color ColorAdd(Color colorA, Color colorB) {
            Color newColor = new Color(0, 0, 0);
            newColor = colorA;
            newColor.r += colorB.r;
            newColor.g += colorB.g;
            newColor.b += colorB.b;
            newColor.a += colorB.a;
            return newColor;
        }

        public static Color ColorSub(Color colorA, Color colorB) {
            Color newColor = new Color(0, 0, 0);
            newColor = colorA;
            newColor.r -= colorB.r;
            newColor.g -= colorB.g;
            newColor.b -= colorB.b;
            newColor.a -= colorB.a;
            return newColor;
        }

        public static Color ColorMul(Color colorA, Color colorB) {
            Color newColor = new Color(0, 0, 0);
            newColor = colorA;
            newColor.r *= colorB.r;
            newColor.g *= colorB.g;
            newColor.b *= colorB.b;
            newColor.a *= colorB.a;
            return newColor;
        }

        public static Color ColorDiv(Color colorA, Color colorB) {
            Color newColor = new Color(0, 0, 0);
            newColor = colorA;
            newColor.r /= colorB.r;
            newColor.g /= colorB.g;
            newColor.b /= colorB.b;
            newColor.a /= colorB.a;
            return newColor;
        }

        public static void LabelBold(string content, params GUILayoutOption[] options) {
            var style = GUI.skin.label;
            style.fontStyle = FontStyle.Bold;
            GUILayout.Label(content, options);
            style.fontStyle = FontStyle.Normal;
        }

        public static Color ResetColor() {
            if (EditorGUIUtility.isProSkin) {
                return proBGColor;
            }

            else {
                return BGColor;
            }
        }

        public static Color PlaymodeTint() {
            try {
                if (Application.isPlaying) {
                    string[] playmodeTintArray = EditorPrefs.GetString("Playmode tint").Split(';');
                    return new Color(float.Parse(playmodeTintArray[1]), float.Parse(playmodeTintArray[2]), float.Parse(playmodeTintArray[3]), float.Parse(playmodeTintArray[4]));
                }
            }

            catch {
                Debug.Log("Cannot get playmode tint.");
            }

            return new Color(1, 1, 1);
        }

        public static Texture2D UniformColorTexture(int w, int h, Color color) {
            Texture2D tex = new Texture2D(w, h);

            for (int i = 0; i < tex.width; i++) {
                for (int j = 0; j < tex.height; j++) {
                    tex.SetPixel(i, j, color);
                }
            }

            tex.Apply();
            return tex;
        }
    }

}