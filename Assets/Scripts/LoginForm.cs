using System.Diagnostics;
using UnityEngine;
using UnityEngine.UIElements;

public class LoginForm : MonoBehaviour
{
    private VisualElement root;
    private TextField emailField;
    private TextField passwordField;
    private Button loginButton;
    private VisualElement background;

    private const string emailPlaceholder = "Enter your email...";
    private const string passwordPlaceholder = "Enter your password...";
    private const string emailError = "Please enter your email";
    private const string passwordError = "Please enter your password";

    void OnEnable()
    {
        Initialize();
    }

    public void Initialize()
    {
        var uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;
        emailField = root.Q<TextField>("emailForm") ?? new TextField { name = "emailForm" };
        passwordField = root.Q<TextField>("passwordForm") ?? new TextField { name = "passwordForm" };
        loginButton = root.Q<Button>("button") ?? new Button { name = "button" };

        if (!root.Contains(emailField)) root.Add(emailField);
        if (!root.Contains(passwordField)) root.Add(passwordField);
        if (!root.Contains(loginButton)) root.Add(loginButton);

        InitTextField(emailField, emailPlaceholder);
        InitTextField(passwordField, passwordPlaceholder);

        loginButton.clicked += OnLoginButtonClicked;

        // Initialize the background element
        background = root.Q<VisualElement>("background");
    }

    private void InitTextField(TextField textField, string placeholderText)
    {
        textField.value = placeholderText;
        textField.RegisterCallback<FocusEvent>(ev => ClearPlaceholder(textField, placeholderText));
        textField.RegisterCallback<BlurEvent>(ev => SetPlaceholder(textField, placeholderText));
    }

    private void ClearPlaceholder(TextField textField, string placeholderText)
    {
        if (textField.value == placeholderText)
        {
            textField.value = "";
        }
    }

    private void SetPlaceholder(TextField textField, string originalPlaceholder)
    {
        if (string.IsNullOrEmpty(textField.value))
        {
            textField.value = originalPlaceholder;
        }
    }

    public void OnLoginButtonClicked()
    {
        bool isEmailValid = ValidateField(emailField, emailPlaceholder, emailError);
        bool isPasswordValid = ValidateField(passwordField, passwordPlaceholder, passwordError);

        if (isEmailValid && isPasswordValid)
        {
            root.style.display = DisplayStyle.None;

            // Display the background image
            if (background != null)
            {
                background.style.display = DisplayStyle.Flex;
            }
        }
        
    }

    private bool ValidateField(TextField field, string placeholderText, string errorMessage)
    {
        if (string.IsNullOrWhiteSpace(field.value) || field.value == placeholderText || field.value == errorMessage)
        {
            field.value = errorMessage;
            field.style.color = GetRandomColor();
            return false;
        }
        return true;
    }

    private Color GetRandomColor()
    {
        return new Color(255, 0, 0);
    }

    void OnDestroy()
    {
        if (emailField != null)
            emailField.UnregisterCallback<FocusEvent>(ev => ClearPlaceholder(emailField, emailPlaceholder));

        if (passwordField != null)
            passwordField.UnregisterCallback<BlurEvent>(ev => SetPlaceholder(passwordField, passwordPlaceholder));

        if (loginButton != null)
            loginButton.clicked -= OnLoginButtonClicked;
    }
}
