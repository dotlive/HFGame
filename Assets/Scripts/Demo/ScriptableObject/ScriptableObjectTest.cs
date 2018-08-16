using UnityEngine;
using System.Collections;

public class ScriptableObjectTest : MonoBehaviour
{
    void Start()
    {
        ContactInfo contact = Resources.Load<ContactInfo>("contact_info") as ContactInfo;
        if (null != contact)
        {
            Debug.Log(contact.fullName);
        }
    }
}
