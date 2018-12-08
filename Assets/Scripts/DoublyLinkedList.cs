using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoublyLinkedListNode
{
    public DoublyLinkedListNode prev;
    public DoublyLinkedListNode forward;
    public Unit item;

    public DoublyLinkedListNode(Unit item, DoublyLinkedListNode prev = null, DoublyLinkedListNode forward = null)
    {
        this.item = item;
        this.prev = prev;
        this.forward = forward;
    }

    public void RemoveItem()
    {
        if (prev != null)
            prev.forward = forward;
        if (forward != null)
            forward.prev = prev;
    }
}
