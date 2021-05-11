using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

    protected Animator animator;
    protected bool shouldDestroy = false;

    public virtual void Open(bool animate = true) {
        gameObject.SetActive(true);
        animator = GetComponent<Animator>();
        if (animator != null) {
            animator.Play("OpenMenu", 0, 0);
        } else {
            print("No Animator on Menu Found");
        }
    }

    /// This method should be called when the OpenMenu animation has compeleted.
    public virtual void DidOpen() { }

    public virtual void Close(bool shouldDestroy, bool animate = true) {
        this.shouldDestroy = shouldDestroy;
        animator = GetComponent<Animator>();
        if (animator != null) {
            animator.Play("CloseMenu", 0, 0);
        } else {
            print("No Animator on Menu Found");
            DestoryMenu();
        }
    }

    /// This method should be called when CloseMenu animation has completed.
    public void DestoryMenu() {
        if (shouldDestroy) {
            Destroy(this.gameObject);
        } else {
            gameObject.SetActive(false);
        }
    }
}
