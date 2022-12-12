using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GetRotationMul : MonoBehaviour
{
    
    [System.Serializable]
    public class BoneMatching
    {
        public AnubisBones AnubisBone;
        public string CharacterBone;
        public Quaternion BaseRot;
    }
    public BoneMatching[] bones = new BoneMatching[22];
    private Vector3 offset_position;

    Actor Actor;

    private void Awake()
    {
        Actor = GameObject.Find("Anubis").GetComponent<Actor>();
    }

    // Start is called before the first frame update
    void Start()
    {
        SetPosition();
        SetBaseRot();

        //var a = bones[0].AnubisBone.ToString();
    }
    

    // Update is called once per frame
    void Update()
    {
        RetargetingBones();
    }

    private void SetPosition()
    {
        offset_position = Actor.transform.position*2 - transform.position;
    }

    private void SetBaseRot()
    {
        for (int i = 0; i < bones.Length; i++)
        {
            var characterBoneTransform = FindTransform(bones[i].CharacterBone);
            var characterBoneRotation = characterBoneTransform.rotation;
            var modelBoneTransform = Actor.FindTransform(bones[i].AnubisBone.ToString());
            var modelBoneRotation = modelBoneTransform.rotation;
            bones[i].BaseRot = Quaternion.Inverse(modelBoneRotation) * characterBoneRotation;
        }
    }

    private void RetargetingBones()
    {
        transform.position = Actor.transform.position*2 - offset_position;
        for (int i = 0; i < bones.Length; i++)
        {
            var bonename = bones[i].CharacterBone;
            var boneTransform = FindTransform(bonename);
            var modelTransform = Actor.FindTransform(bones[i].AnubisBone.ToString());
            boneTransform.rotation = modelTransform.rotation * bones[i].BaseRot;
        }
    }

    public Transform FindTransform(string name)
    {
        Transform element = null;
        Action<Transform> recursion = null;
        recursion = new Action<Transform>((transform) => {
            if (transform.name == name)
            {
                element = transform;
                return;
            }
            for (int i = 0; i < transform.childCount; i++)
            {
                recursion(transform.GetChild(i));
            }
        });
        recursion(GetRoot());
        return element;
    }
    public Transform GetRoot()
    {
        return transform;
    }
}

public enum AnubisBones
{

    Hips, Chest, Chest2, Chest3, Chest4, Neck, Head, 
    RightCollar, RightShoulder, RightElbow, RightWrist,
    LeftCollar, LeftShoulder, LeftElbow, LeftWrist,
    RightHip, RightKnee, RightAnkle, RightToe,
    LeftHip, LeftKnee, LeftAnkle, LeftToe

}