using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/// Interface used to display a stack of menus.
public interface IMenuStack {
    /// How many elements are int the stack.
    int Count { get; }

    /// True if the stack is empty
    bool IsEmpty { get; }

    /// Adds a menu to the menu stack, this menu will be visable.
    Menu PushMenu(Menu menu);

    /// Sorts through the stack and removes the given menu, 
    /// this will only animate the menu if it is the top of the stack.
    void RemoveMenu(Menu menu);

    /// Removes and returns the top menu from off of the stack.
    void PopMenu();

    /// Allows another class to view the menu on the top of the stack.
    Menu PeekMenu();

    /// Removes all of the menus from the stack and closes the menu stack view.
    void CloseMenuStack();

    /// Whether or not the menu is already contained within the stack.
    bool Contains(Menu menu);
}

/// The menu stack is a concrete repretation of the IMenuStack interface 
/// intended to be a monobehavior on the canvas or a subview of the canvas.
/// This class manages menus being added and removed from the screen.
public class MenuStack : MonoBehaviour, IMenuStack {

#pragma warning disable 0649

    [SerializeField]
    private Image menuStackBackground;

    /// Internal stack used for the Menu Stack to manage the menus given to it.
    private Stack<Menu> menuStack = new Stack<Menu>();

    public int Count => menuStack.Count;

    public bool IsEmpty => menuStack.Count == 0;

    public event Action StackStarted;
    public event Action StackEnded;
    public event Action MenuAdded;
    public event Action MenuRemoved;

#pragma warning restore 0649

    public Menu PushMenu(Menu menu) {
        if (menuStack.Count == 0) {
            StartStack();
        } else {
            HidePreviousMenu();
        }

        Menu newMenu = Instantiate(menu);
        newMenu.transform.SetParent(this.transform);
        newMenu.transform.localScale = new Vector3(1, 1, 0);

        newMenu.Open();

        menuStack.Push(newMenu);
        MenuAdded?.Invoke();

        return newMenu;
    }

    public void RemoveMenu(Menu menu) {
        if (menuStack.Contains(menu)) {
            Stack<Menu> tempStack = new Stack<Menu>();
            Menu poppedMenu = menuStack.Pop();

            while (poppedMenu != menu && menuStack.Count != 0) {
                tempStack.Push(poppedMenu);
                poppedMenu = menuStack.Pop();
            }

            poppedMenu.Close(true, tempStack.Count == 0);

            while (tempStack.Count > 0) {
                menuStack.Push(tempStack.Pop());
            }

            MenuRemoved?.Invoke();
        }
    }

    public void PopMenu() {
        if(menuStack.Count == 0) {
            return;
        }

        Menu removedMenu = menuStack.Pop();
        removedMenu.Close(true);

        if (menuStack.Count == 0) {
            EndStack();
        } else {
            ShowPreviousMenu();
        }

        MenuRemoved?.Invoke();
    }

    public Menu PeekMenu() {
        if(menuStack.Count > 0) {
            return menuStack.Peek();
        }

        return null;
    }

    public void CloseMenuStack() {
        if(menuStack.Count == 0) {
            return;
        }

        Menu removedMenu = menuStack.Pop();
        removedMenu.Close(true);

        foreach (Menu menu in menuStack) {
            Destroy(menu.gameObject);
        }
        menuStack = new Stack<Menu>();

        EndStack();
    }

    public bool Contains(Menu menu) {
        foreach (Menu ownedMenu in menuStack) {
            if (ownedMenu.GetType() == menu.GetType()) {
                return true;
            }
        }

        return false;
    }

    /// After a menu is closed the previous one should be shown.
    private void ShowPreviousMenu() {
        if (menuStack.Count > 0) {
            menuStack.Peek().Open();
        }
    }

    /// This should be called before a new menu is added.
    private void HidePreviousMenu() {
        if (menuStack.Count > 0) {
            Menu removedMenu = menuStack.Peek();
            removedMenu.Close(false);
        }
    }

    private void StartStack() {
        StackStarted?.Invoke();
        menuStackBackground.gameObject.SetActive(true);
    }

    private void EndStack() {
        StackEnded?.Invoke();
        menuStackBackground.gameObject.SetActive(false);
    }
}
