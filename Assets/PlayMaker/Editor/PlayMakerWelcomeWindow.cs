// (c) Copyright HutongGames, LLC 2010-2011. All rights reserved.

#if (UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5_0) 
#define UNITY_PRE_5_1
#endif

using System;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace HutongGames.PlayMakerEditor
{
    /// <summary>
    /// Welcome Window with getting started shortcuts
    /// </summary>
    [InitializeOnLoad]
    public class PlayMakerWelcomeWindow : EditorWindow
    {
        private const string installCurrentVersion = "1.7.8.4";
        private const string installBetaVersion = "1.8.0 BETA RC37";

        private const string editorPrefsShowAtStartup = "PlayMaker.ShowWelcomeScreen.1.7.8.4"; // Add version to ensure first time open
        private const string editorPrefsSavedPage = "PlayMaker.WelcomeScreenPage";
        private const string editorPrefsShowUpgradeGuide = "PlayMaker.ShowUpgradeGuide";
        private const string urlSamples = "http://www.hutonggames.com/samples.php";
        private const string urlTutorials = "http://www.hutonggames.com/tutorials.html";
        private const string urlDocs = "https://hutonggames.fogbugz.com/default.asp?W1";
        private const string urlForums = "http://hutonggames.com/playmakerforum/index.php";
        private const string urlPhotonAddon = "https://hutonggames.fogbugz.com/default.asp?W928";
        private const string urlAddonsWiki = "https://hutonggames.fogbugz.com/default.asp?W714";
        private const string urlEcosystemWiki = "https://hutonggames.fogbugz.com/default.asp?W1181";
        //private const string urlStore = "http://www.hutonggames.com/store.html";
        //private const string photonID = "1786";

        private const float windowWidth = 500;
        private const float windowHeight = 440;
        private const float pageTop = 70;
        private const float pagePadding = 95;

        private static bool setupPhoton;

        private static string currentVersion;
        private static string currentVersionLabel;
        private static string currentVersionShort;
        private static int majorVersion; // 1.8 -> 18 for easier comparisons

        private enum Page
        {
            Welcome = 0,
            Install = 1,
            GettingStarted = 2,
            UpgradeGuide = 3,
            Addons = 4
        }
        private Page currentPage = Page.Welcome;
        private Page nextPage;
        private Rect currentPageRect;
        private Rect nextPageRect;
        private float currentPageMoveTo;
        private Rect headerRect;
        private Rect backButtonRect;

        private bool pageInTransition;
        private float transitionStartTime;
        private const float transitionDuration = 0.5f;

        private Vector2 scrollPosition;
        private static bool showAtStartup;
        
        private static GUIStyle playMakerHeader;
        private static GUIStyle labelWithWordWrap;
        private static GUIStyle largeTitleWithLogo;
        private static GUIStyle versionLabel;
        private static Texture samplesIcon;
        private static Texture docsIcon;
        private static Texture videosIcon;
        private static Texture forumsIcon;
        private static Texture addonsIcon;
        private static Texture photonIcon;
        private static Texture backButton;

        private static bool stylesInitialized;

        [MenuItem("PlayMaker/Welcome Screen", false, 45)]
        public static void OpenWelcomeWindow()
        {
            GetWindow<PlayMakerWelcomeWindow>(true);
        }

        static PlayMakerWelcomeWindow()
        {
            EditorApplication.playmodeStateChanged -= OnPlayModeChanged;
            EditorApplication.playmodeStateChanged += OnPlayModeChanged;

            showAtStartup = EditorPrefs.GetBool(editorPrefsShowAtStartup, true);
            if (showAtStartup)
            {
                // Delay until first update
                EditorApplication.update -= OpenAtStartup;
                EditorApplication.update += OpenAtStartup;
            }
        }

        static void OnPlayModeChanged()
        {
            //Debug.Log("OnPlayModeChanged - remove welcome callback");

            // don't show welcome screen on playmode change
            EditorApplication.update -= OpenAtStartup;
            EditorApplication.playmodeStateChanged -= OnPlayModeChanged;
        }

        static void OpenAtStartup()
        {
            OpenWelcomeWindow();
            EditorApplication.update -= OpenAtStartup;
        }

        public void OnEnable()
        {
#if UNITY_PRE_5_1
            title = "Welcome To PlayMaker";
#else
            titleContent = new GUIContent("Welcome To PlayMaker");            
#endif
            maxSize = new Vector2(windowWidth, windowHeight);
            minSize = maxSize;
 
            // Try to get current playmaker version if installed
            GetPlayMakerVersion();

            // Is PlayMakerPhotonWizard available?
            setupPhoton = GetType("PlayMakerPhotonWizard") != null;

            // Setup pages

            currentPageRect = new Rect(0, pageTop, windowWidth, windowHeight - pagePadding);
            nextPageRect = new Rect(0, pageTop, windowWidth, windowHeight - pagePadding);
            headerRect = new Rect(0, 0, windowWidth, 60);
            backButtonRect = new Rect(0, windowHeight-24, 123, 24);

            // Save page to survive recompile...?
            //currentPage = (Page)EditorPrefs.GetInt(editorPrefsPage, (int)Page.Welcome);
            
            currentPage = Page.Welcome;

            // We want to show the Upgrade Guide after installing
            // However, installation forces a recompile so we save an EditorPref

            var showUpgradeGuide = EditorPrefs.GetBool(editorPrefsShowUpgradeGuide, false);
            if (showUpgradeGuide)
            {
                //currentPage = Page.UpgradeGuide; //TODO: This was problematic, need a better solution
                EditorPrefs.SetBool(editorPrefsShowUpgradeGuide, false); // reset
                EditorUtility.DisplayDialog("PlayMaker",
                    "Please check the Upgrade Guide for more information on this release.", 
                    "OK");
            }

            SetPage(currentPage);               
            Update();
        }

        private void GetPlayMakerVersion()
        {
            var versionInfo = GetType("HutongGames.PlayMakerEditor.VersionInfo");
            if (versionInfo != null)
            {
                currentVersion = versionInfo.GetMethod("GetAssemblyInformationalVersion").Invoke(null, null) as string;
                if (currentVersion != null)
                {
                    currentVersionShort = currentVersion.Substring(0, currentVersion.LastIndexOf('.'));
                    currentVersionLabel = "version " + currentVersionShort;
                    majorVersion = int.Parse(currentVersionShort.Substring(0, 3).Replace(".", ""));
                }
                else
                {
                    currentVersionLabel = "version unkown";
                    currentVersionShort = "";
                    majorVersion = -1;
                }
            }
            else
            {
                currentVersionLabel = "Not installed";
                currentVersionShort = "";
                majorVersion = -1;
            }
        }

        private void InitStyles()
        {
            if (!stylesInitialized)
            {
                playMakerHeader = new GUIStyle
                {
                    normal =
                    {
                        background = Resources.Load("playMakerHeader") as Texture2D,
                        textColor = Color.white
                    },
                    border = new RectOffset(253, 0, 0, 0),
                };

                largeTitleWithLogo = new GUIStyle
                {
                    normal =
                    {
                        background = Resources.Load("logoHeader") as Texture2D,
                        textColor = Color.white
                    },
                    border = new RectOffset(60, 0, 0, 0),
                    padding = new RectOffset(60, 0, 0, 0),
                    margin = new RectOffset(0, 0, 0, 0),
                    contentOffset = new Vector2(0, 0),
                    alignment = TextAnchor.MiddleLeft,
                    fixedHeight = 60,
                    fontSize = 36,
                    fontStyle = FontStyle.Bold,
                };

                labelWithWordWrap = new GUIStyle(EditorStyles.label) { wordWrap = true };
                versionLabel = new GUIStyle(EditorStyles.label) { alignment = TextAnchor.LowerRight};

                samplesIcon = (Texture) Resources.Load("linkSamples");
                videosIcon = (Texture) Resources.Load("linkVideos");
                docsIcon = (Texture) Resources.Load("linkDocs");
                forumsIcon = (Texture) Resources.Load("linkForums");
                addonsIcon = (Texture) Resources.Load("linkAddons");
                photonIcon = (Texture) Resources.Load("photonIcon");
                backButton = (Texture) Resources.Load("backButton");
            }
            stylesInitialized = true;
        }

        public void OnGUI()
        {
            InitStyles();

            GUILayout.BeginVertical();
            
            DoHeader();

            GUILayout.BeginVertical();

            DoPage(currentPage, currentPageRect);
            
            if (pageInTransition)
            {
                DoPage(nextPage, nextPageRect);
            }

            // Bottom line

            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal();                
            GUILayout.FlexibleSpace();

            var show = GUILayout.Toggle(showAtStartup, "Show At Startup");
            if (show != showAtStartup)
            {
                showAtStartup = show;
                EditorPrefs.SetBool(editorPrefsShowAtStartup, showAtStartup);
            }

            GUILayout.Space(10);
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
            GUILayout.EndVertical();

            if (currentPage != Page.Welcome && !pageInTransition)
            {
                DoBackButton(Page.Welcome);
            }
        }

        private void DoHeader()
        {
            switch (nextPage)
            {
                case Page.Welcome:
                    GUI.Box(headerRect, "", playMakerHeader);
                    break;

                case Page.Install:
                    GUI.Box(headerRect, "Installation", largeTitleWithLogo);
                    break;

                case Page.GettingStarted:
                    GUI.Box(headerRect, "Getting Started", largeTitleWithLogo);
                    break;

                case Page.UpgradeGuide:
                    GUI.Box(headerRect, "Upgrade Guide", largeTitleWithLogo);
                    break;

                case Page.Addons:
                    GUI.Box(headerRect, "Add-Ons", largeTitleWithLogo);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            // Version
            if (!string.IsNullOrEmpty(currentVersion) && majorVersion > 17)
            {
                GUI.Box(headerRect, currentVersionLabel, versionLabel);
            }

            // reserve space in layout
            GUILayoutUtility.GetRect(position.width, 60);
        }

        private void DoPage(Page page, Rect pageRect)
        {
            pageRect.height = position.height - pagePadding;
            GUILayout.BeginArea(pageRect);

            switch (page)
            {
                case Page.Welcome:
                    DoWelcomePage();
                    break;
                case Page.Install:
                    DoInstallPage();
                    break;
                case Page.GettingStarted:
                    DoGettingStartedPage();
                    break;
                case Page.UpgradeGuide:
                    DoUpgradeGuidePage();
                    break;
                case Page.Addons:
                    DoAddonsPage();
                    break;
            }

            GUILayout.EndArea();
        }

        private void DoWelcomePage()
        {
            GUILayout.BeginVertical();
            GUILayout.FlexibleSpace();

            DrawLink(samplesIcon,
                     "Install PlayMaker",
                     "Import the latest version of PlayMaker.",
                     GotoPage, Page.Install);

            DrawLink(docsIcon,
                     "Upgrade Guide",
                     "Guide to upgrading Unity/PlayMaker.",
                     GotoPage, Page.UpgradeGuide);

            DrawLink(videosIcon,
                     "Getting Started",
                     "Links to samples, tutorials, forums etc.",
                     GotoPage, Page.GettingStarted);

            DrawLink(addonsIcon,
                 "Add-Ons",
                 "Extend PlayMaker with these powerful add-ons.",
                 GotoPage, Page.Addons);

            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
        }

        private void DoInstallPage()
        {
            ShowBackupHelpBox();

            GUILayout.BeginVertical();
            GUILayout.Space(30);

            DrawLink(samplesIcon,
                     "Install PlayMaker " + installCurrentVersion,
                     "The current official release.",
                     InstallLatest, null);

            DrawLink(samplesIcon,
                     "Install PlayMaker " + installBetaVersion,
                     "The latest public beta version.",
                     InstallBeta, null);

            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
        }

        private void DoGettingStartedPage()
        {
            GUILayout.BeginVertical();
            GUILayout.FlexibleSpace();

            DrawLink(samplesIcon,
                 "Samples",
                 "Download sample scenes and complete projects.",
                 OpenUrl, urlSamples);

            DrawLink(videosIcon,
                 "Tutorials",
                 "Watch tutorials on the PlayMaker YouTube channel.",
                 OpenUrl, urlTutorials);

            DrawLink(docsIcon,
                 "Docs",
                 "Browse the online manual.",
                 OpenUrl, urlDocs);

            DrawLink(forumsIcon,
                 "Forums",
                 "Join the PlayMaker community!",
                 OpenUrl, urlForums);

            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
        }

        private void DoUpgradeGuidePage()
        {
            ShowBackupHelpBox();

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            GUILayout.Label("Version 1.8.0", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox("FSMs saved with 1.8.0 cannot be opened in earlier versions of PlayMaker! Please BACKUP projects!", MessageType.Warning);

            GUILayout.Label("Unity 5 Upgrade Notes", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox("If you run into problems updating a Unity 4.x project please check the Troubleshooting guide on the PlayMaker Wiki.", MessageType.Warning);
            EditorGUILayout.HelpBox("Unity 5 removed component property shortcuts from GameObject. " +
                                    "\n\nThe Unity auto update process replaces these properties with GetComponent calls. " +
                                    "In many cases this is fine, but some third party actions and addons might need manual updating! " +
                                    "Please post on the PlayMaker forums and contact the original authors for help." +
                                    "\n\nIf you used these GameObject properties in Get Property or Set Property actions " +
                                    "they are no longer valid, and you need to instead point to the Component directly. " +
                                    "E.g., Drag the Component (NOT the GameObject) into the Target Object field." +
                                    "\n", MessageType.Warning);

            GUILayout.Label("Unity 4.6 Upgrade Notes", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox("Find support for the new Unity GUI online in our Addons page.", MessageType.Info);
            EditorGUILayout.HelpBox("PlayMakerGUI is only needed if you use OnGUI Actions. " +
                                    "If you don't use OnGUI actions un-check Auto-Add PlayMakerGUI in PlayMaker Preferences.", MessageType.Info);

            EditorGUILayout.EndScrollView();
            //FsmEditorGUILayout.Divider();
        }

        private void DoAddonsPage()
        {
            GUILayout.BeginVertical();
            GUILayout.FlexibleSpace();
            if (setupPhoton)
            {
                DrawLink(photonIcon,
                     "Photon Cloud",
                     "Build scalable MMOGs, FPS or any other multiplayer game " +
                     "and application for PC, Mac, Browser, Mobile or Console.",
                     LaunchPhotonSetupWizard, null);
            }
            else
            {
                DrawLink(photonIcon,
                     "Photon Cloud",
                     "Build scalable MMOGs, FPS or any other multiplayer game " +
                     "and application for PC, Mac, Browser, Mobile or Console.",
                     OpenUrl, urlPhotonAddon);
            }

            DrawLink(addonsIcon,
                 "Ecosystem",
                 "An integrated online browser for custom actions, samples and addons.",
                 OpenUrl, urlEcosystemWiki);

            DrawLink(addonsIcon,
                 "Add-Ons",
                 "Find action packs and add-ons for NGUI, 2D Toolkit, Mecanim, Pathfinding, Smooth Moves, Ultimate FPS...",
                 OpenUrl, urlAddonsWiki);

            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();

        }

        private static void ShowBackupHelpBox()
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.HelpBox("Always BACKUP projects before updating!\nUse Version Control to manage changes!", MessageType.Error);
            GUILayout.Space(5);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            EditorGUILayout.HelpBox("With Unity 5.3 please install the 1.8.0 Beta Version to address compatibility issues!", MessageType.Error);
            GUILayout.Space(5);
            GUILayout.EndHorizontal();
        }

        private static void DrawLink(Texture texture, string heading, string body, LinkFunction func, object userData)
        {
            GUILayout.BeginHorizontal();

            GUILayout.Space(64);
            GUILayout.Box(texture, GUIStyle.none, GUILayout.MaxWidth(48));
            GUILayout.Space(10);

            GUILayout.BeginVertical();
            GUILayout.Space(1);
            GUILayout.Label(heading, EditorStyles.boldLabel);
            GUILayout.Label(body, labelWithWordWrap);
            GUILayout.EndVertical();

            GUILayout.EndHorizontal();

            var rect = GUILayoutUtility.GetLastRect();
            EditorGUIUtility.AddCursorRect(rect, MouseCursor.Link);

            if (Event.current.type == EventType.mouseDown && rect.Contains(Event.current.mousePosition))
            {
                func(userData);
            }

            GUILayout.Space(10);
        }

        private void DoBackButton(Page toPage)
        {
            GUI.Box(backButtonRect, backButton, GUIStyle.none);
            EditorGUIUtility.AddCursorRect(backButtonRect, MouseCursor.Link);

            if (Event.current.type == EventType.mouseDown && backButtonRect.Contains(Event.current.mousePosition))
            {
                GotoPage(toPage);
                GUIUtility.ExitGUI();
            }
        }

        void Update()
        {
            if (pageInTransition)
            {
                DoPageTransition();
            }
        }

        void DoPageTransition()
        {
            var t = (Time.realtimeSinceStartup - transitionStartTime) / transitionDuration;
            if (t > 1f)
            {
                SetPage(nextPage);
                return;
            }

            var nextPageX = Mathf.SmoothStep(nextPageRect.x, 0, t);
            var currentPageX = Mathf.SmoothStep(currentPageRect.x, currentPageMoveTo, t);
            currentPageRect.Set(currentPageX, pageTop, windowWidth, position.height);
            nextPageRect.Set(nextPageX, pageTop, windowWidth, position.height);

            Repaint();
        }

        bool DisplayInstallDialog(string versionInfo, string notes)
        {
            return EditorUtility.DisplayDialog("PlayMaker", "Install PlayMaker " + versionInfo + "\n" + 
                notes + "\n\nAlways backup projects before updating Unity or PlayMaker!",
                "I Made a Backup. Go Ahead!", "Cancel");
        }

        // Button actions:

        public delegate void LinkFunction(object userData);

        private void InstallLatest(object userData)
        {
            if (majorVersion > 17 && EditorUtility.DisplayDialog("PlayMaker", "This project uses a newer version of PlayMaker not compatible with 1.7.8.4. Installing 1.7.8.4 is NOT a good idea!", "Cancel", "Do it anyway!"))
            {
                return;
            }

            if (DisplayInstallDialog(installCurrentVersion, "The latest release version of PlayMaker."))
            {
                EditorPrefs.SetBool(editorPrefsShowUpgradeGuide, true);
                //ImportPackage("Assets/PlayMaker/Editor/Install/PlayMaker.1.7.8.4.unitypackage");
                // Use GUID in case user has moved Playmaker folder
                ImportPackage(AssetDatabase.GUIDToAssetPath("dd583cbbf618ba54983cdf396b28e49b"));
            }
        }

        private void InstallBeta(object userData)
        {
            if (DisplayInstallDialog(installBetaVersion, "The latest BETA version of PlayMaker." +
                                                        "\n\nNOTE: Projects saved with PlayMaker 1.8.0 cannot be opened in older versions of PlayMaker!"))
            {
                EditorPrefs.SetBool(editorPrefsShowUpgradeGuide, true);
                //ImportPackage("Assets/PlayMaker/Editor/Install/PlayMaker.1.8.0.unitypackage");
                // Use GUID in case user has moved Playmaker folder
                ImportPackage(AssetDatabase.GUIDToAssetPath("f982487afa4f0444ea11e90a9d05b94e"));
            }
        }

        private void ImportPackage(string package)
        {
            try
            {
                AssetDatabase.ImportPackage(package, true);
            }
            catch (Exception)
            {
                Debug.LogError("Failed to import package: " + package);
                throw;
            }

            // This didn't work that well
            // Instead let the user open the upgrade guide
            //GotoPage(Page.UpgradeGuide);
        }

        private void LaunchPhotonSetupWizard(object userData)
        {
            GetType("PlayMakerPhotonWizard").GetMethod("Init").Invoke(null, null);
        }

        private void OpenUrl(object userData)
        {
            Application.OpenURL(userData as string);
        }

        public void OpenInAssetStore(object userData)
        {
            AssetStore.Open("content/" + userData);
        }

        private void GotoPage(object userData)
        {
            nextPage = (Page)userData;
            pageInTransition = true;
            transitionStartTime = Time.realtimeSinceStartup;

            // next page slides in from the right
            // welcome screen slides offscreen left
            // reversed if returning to the welcome screen

            if (nextPage == Page.Welcome)
            {
                nextPageRect.x = -windowWidth;
                currentPageMoveTo = windowWidth;
            }
            else
            {
                nextPageRect.x = windowWidth;
                currentPageMoveTo = -windowWidth;
            }

            GUIUtility.ExitGUI();
        }

        private void SetPage(Page page)
        {
            currentPage = page;
            nextPage = page;
            pageInTransition = false;
            currentPageRect.x = 0;
            SaveCurrentPage();
            Repaint();
        }

        private void SaveCurrentPage()
        {
            EditorPrefs.SetInt(editorPrefsSavedPage, (int)currentPage);
        }

        // Normally we would use ReflectionUtils.GetGlobalType but this window now needs to be standalone
        // Instead of copy/pasting ReflectionUtils, decided to try this code from UnityAnswers:
        // http://answers.unity3d.com/questions/206665/typegettypestring-does-not-work-in-unity.html
        public static Type GetType(string typeName)
        {
            // Try Type.GetType() first. This will work with types defined
            // by the Mono runtime, in the same assembly as the caller, etc.
            var type = Type.GetType(typeName);

            // If it worked, then we're done here
            if (type != null)
                return type;

            // otherwise look in loaded assemblies
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = assembly.GetType(typeName);
                if (type != null)
                {
                    break;
                }
            }

            return type;
        }

        public static Type FindTypeInLoadedAssemblies(string typeName)
        {
            Type _type = null;
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                _type = assembly.GetType(typeName);
                if (_type != null)
                    break;
            }

            return _type;
        }
    }
}