using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class EXP_MenuManager : MonoBehaviour {

    public RA_Menu RAMenuPrefab;
    public RA_Menu_II RAMenuIIPrefab;
    public DebriefMenu DebriefMenuPrefab;
    public InstructionsMenu InstructionsMenuPrefab;

    private Stack<Menu> MenuStack = new Stack<Menu>();

    private RA_Menu_II RA_Menu_II;

    public static EXP_MenuManager Instance { get; private set; }

    private void Awake() {
        Instance = this;
        //RA_Menu.Show();

    }

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void CreateInstance<T>() where T : Menu {
        var prefab = GetPrefab<T>();

        Menu menu = Instantiate(prefab, transform);

        if(typeof(T) == typeof(RA_Menu_II)) {
            RA_Menu_II = menu.gameObject.GetComponent<RA_Menu_II>();
        }

    }

    private void OnDestroy() {
        Instance = null;
    }

    public void OpenMenu(Menu instance) {

        // Deactivate top menu
        if (MenuStack.Count > 0) {

            if (instance.DisableMenusUnderneath) {

                foreach (var menu in MenuStack) {
                    menu.gameObject.SetActive(false);

                    if (menu.DisableMenusUnderneath)
                        break;
                }

            }

            var topCanvas = instance.GetComponent<Canvas>();
            var previousCanvas = MenuStack.Peek().GetComponent<Canvas>();
            topCanvas.sortingOrder = previousCanvas.sortingOrder + 1;

        }

        MenuStack.Push(instance);

    }

    /// <summary>
    /// Closes the current menu and remove it from the stack
    /// </summary>
    public void CloseMenu() {
        var instance = MenuStack.Pop();

        Destroy(instance.gameObject);

        // Re-activate top mneu
        if (MenuStack.Count > 0) {
            MenuStack.Peek().gameObject.SetActive(true);
            // Setting top of menu stack to active
            //Debug.Log("Setting top of menu stack to active");
        }

    }

    /// <summary>
    /// Get Prefab dynamically, based on public fields set from Unity
    /// Private fields with SerializeField attribute is also an option
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    private T GetPrefab<T>() where T : Menu {
        var fields = this.GetType().GetFields(BindingFlags.Public |
            BindingFlags.Instance | BindingFlags.DeclaredOnly);

        // WE NEED TO AVOID FOREACH AS MUCH AS POSSIBLE
        foreach (var field in fields) {
            var prefab = field.GetValue(this) as T;

            if (prefab != null) {
                return prefab;
            }

        }

        if (typeof(T) == typeof(RA_Menu))
            return RAMenuPrefab as T;


        throw new MissingReferenceException("Prefab not found for type " + typeof(T));
    }

    /// <summary>
    /// Close the selected menu
    /// </summary>
    /// <param name="menu">Our menu object</param>
    public void CloseMenu(Menu menu) {

        if (MenuStack.Count == 0) {
            Debug.LogErrorFormat(menu, "{0} cannot be closed because menu stack is empty", menu.GetType());
            return;
        }

        if (MenuStack.Peek() != menu) {
            Debug.LogErrorFormat(menu, "{0} cannot be closed because it is not on top of stack", menu.GetType());
            return;
        }

        CloseTopMenu();
    }

    /// <summary>
    /// Closes menu at the top of our stack
    /// </summary>
    public void CloseTopMenu() {
        var instance = MenuStack.Pop();

        if (instance.DestroyWhenClosed)
            Destroy(instance.gameObject);
        else
            instance.gameObject.SetActive(false);

        // Reactivate top menu
        // If reactivated menu is an overlay, we need to activate the menu under it
        // TODO: AVOID USING FOREACH
        foreach (var menu in MenuStack) {
            menu.gameObject.SetActive(true);

            if (menu.DisableMenusUnderneath)
                break;

        }
    }


}
