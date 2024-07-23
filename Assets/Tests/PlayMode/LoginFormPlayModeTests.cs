using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UIElements;
using System.Collections;

public class LoginFormPlayModeTests
{
    private GameObject testGameObject;
    private LoginForm loginForm;
    private UIDocument uiDocument;

    [SetUp]
    public void SetUp()
        {
            // Create a temporary GameObject
            testGameObject = new GameObject("TestGameObject");

            // Add and configure the UIDocument
            uiDocument = testGameObject.AddComponent<UIDocument>();
            uiDocument.panelSettings = ScriptableObject.CreateInstance<PanelSettings>();

            // Create and setup the root VisualElement
            var root = new VisualElement() { name = "Root" };
            uiDocument.visualTreeAsset = ScriptableObject.CreateInstance<VisualTreeAsset>();
            uiDocument.visualTreeAsset.CloneTree(root);

            // Add UI elements
            var emailField = new TextField { name = "emailForm" };
            var passwordField = new TextField { name = "passwordForm" };
            var loginButton = new Button { name = "button" };

            root.Add(emailField);
            root.Add(passwordField);
            root.Add(loginButton);

            // Ensure the GameObject is active before adding LoginForm
            testGameObject.SetActive(false);

            // Add LoginForm component
            loginForm = testGameObject.AddComponent<LoginForm>();

            // Manually trigger the OnEnable method
            testGameObject.SetActive(true);
            loginForm.Initialize();
    }


    //***************************************************************************//
    //debut_tests_unitaires
    //verifie s'il y'a de base écrit dans le textfiled "enter your email..."
    [UnityTest]
    public IEnumerator TestEmailPlaceholderInitialization()
    {
        yield return null; // Wait for initialization
        var emailField = uiDocument.rootVisualElement.Q<TextField>("emailForm");
        Assert.AreEqual("Enter your email...", emailField.value);
    }

    //verifie s'il y'a de base écrit dans le textfiled "enter your password..."
    [UnityTest]
    public IEnumerator TestPasswordPlaceholderInitialization()
    {
        yield return null; // Wait for initialization
        var passwordField = uiDocument.rootVisualElement.Q<TextField>("passwordForm");
        Assert.AreEqual("Enter your password...", passwordField.value);
    }

    //verifie si le textfield email n'est pas vide lorsqu'on click sur le boutton
    [UnityTest]
    public IEnumerator TestEmailValidation()
    {
        yield return null;
        var emailField = uiDocument.rootVisualElement.Q<TextField>("emailForm");
        emailField.value = "";
        loginForm.OnLoginButtonClicked();
        Assert.AreEqual("Please enter your email", emailField.value);
    }

    //verifie si le textfield password n'est pas vide lorsqu'on click sur le boutton
    [UnityTest]
    public IEnumerator TestPasswordValidation()
    {
        yield return null;
        var passwordField = uiDocument.rootVisualElement.Q<TextField>("passwordForm");
        passwordField.value = "";
        loginForm.OnLoginButtonClicked();
        Assert.AreEqual("Please enter your password", passwordField.value);
    }

    //verifie si le textfield email est entré MAIS le textfield password est vide lorsqu'on click sur le boutton
    [UnityTest]
    public IEnumerator TestEmailFilledPasswordEmpty()
    {
        yield return null;
        var emailField = uiDocument.rootVisualElement.Q<TextField>("emailForm");
        var passwordField = uiDocument.rootVisualElement.Q<TextField>("passwordForm");

        emailField.value = "test@example.com";
        passwordField.value = "";
        loginForm.OnLoginButtonClicked();

        Assert.AreEqual("test@example.com", emailField.value);
        Assert.AreEqual("Please enter your password", passwordField.value);
    }

    //verifie si le textfield password est entré MAIS le textfield email est vide lorsqu'on click sur le boutton
    [UnityTest]
    public IEnumerator TestPasswordFilledEmailEmpty()
    {
        yield return null;
        var emailField = uiDocument.rootVisualElement.Q<TextField>("emailForm");
        var passwordField = uiDocument.rootVisualElement.Q<TextField>("passwordForm");

        emailField.value = "";
        passwordField.value = "password123";
        loginForm.OnLoginButtonClicked();

        Assert.AreEqual("Please enter your email", emailField.value);
        Assert.AreEqual("password123", passwordField.value);
    }

    //verifie si le textfield password est vide ET le textfield email est vide lorsqu'on click sur le boutton
    [UnityTest]
    public IEnumerator TestBothEmailAndPasswordEmpty()
    {
        yield return null;
        var emailField = uiDocument.rootVisualElement.Q<TextField>("emailForm");
        var passwordField = uiDocument.rootVisualElement.Q<TextField>("passwordForm");

        emailField.value = "";
        passwordField.value = "";
        loginForm.OnLoginButtonClicked();

        Assert.AreEqual("Please enter your email", emailField.value);
        Assert.AreEqual("Please enter your password", passwordField.value);
    }

    //verifie si le textfield password est rempli ET le textfield email est rempli lorsqu'on click sur le boutton
    [UnityTest]
    public IEnumerator TestBothEmailAndPasswordFilled()
    {
        yield return null;
        var emailField = uiDocument.rootVisualElement.Q<TextField>("emailForm");
        var passwordField = uiDocument.rootVisualElement.Q<TextField>("passwordForm");

        emailField.value = "test@example.com";
        passwordField.value = "password123";
        loginForm.OnLoginButtonClicked();

        Assert.AreEqual("test@example.com", emailField.value);
        Assert.AreEqual("password123", passwordField.value);
        Assert.AreEqual(DisplayStyle.None, uiDocument.rootVisualElement.style.display.value);
    }

    [TearDown]
    public void TearDown()
    {
        UnityEngine.Object.DestroyImmediate(testGameObject);
    }
}