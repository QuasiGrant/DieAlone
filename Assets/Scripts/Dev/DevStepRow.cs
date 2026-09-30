#if UNITY_EDITOR || DEVELOPMENT_BUILD
using UnityEngine;
using UnityEngine.EventSystems;

/// A dev panel row that steps through a list in place: Left/Right (arrow keys, d-pad, stick) step back and forward
/// at once, Enter/A and a click on the row step forward. Up and Down move to the next row as usual.
/// DevMenu builds it from code and sets Step. Dev-only: the whole file compiles out of release builds.
public class DevStepRow : UnityEngine.UI.Selectable, ISubmitHandler, IPointerClickHandler
{
    /// Called with -1 or +1.
    public System.Action<int> Step;

    public override void OnMove(AxisEventData eventData)
    {
        if (eventData.moveDir == MoveDirection.Left) { Step?.Invoke(-1); eventData.Use(); return; }
        if (eventData.moveDir == MoveDirection.Right) { Step?.Invoke(1); eventData.Use(); return; }
        base.OnMove(eventData);
    }

    public void OnSubmit(BaseEventData eventData)
    {
        if (IsInteractable()) Step?.Invoke(1);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left && IsInteractable()) Step?.Invoke(1);
    }
}
#endif
