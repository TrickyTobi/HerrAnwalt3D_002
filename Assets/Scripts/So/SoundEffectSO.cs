using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static SurfaceMaterialSelection;

[CreateAssetMenu(menuName = ("SoundEffectSO"))]
public class SoundEffectSO : ScriptableObject
{




    #region Sound Effects
    [Space(20)]
    [Header("Generic Sounds")]
    [Space(10)]

    public AudioClip[] punchSound;
    [Space(10)]
    public AudioClip[] attackSwooshSound;
    [Space(10)]
    public AudioClip[] fenceSound;
    [Space(10)]
    public AudioClip[] fenceStartNoticeSound;
    [Space(10)]
    public AudioClip[] metalGateHitSound;
    [Space(10)]
    public AudioClip[] childNoticeSound;
    [Space(10)]



    [Space(20)]
    [Header("Walk Sounds")]
    [Space(10)]
    public AudioClip[] hardFloor;
    [Space(10)]
    public AudioClip[] grasFloor;
    [Space(10)]
    public AudioClip[] hallwayFloor;
    [Space(10)]
    public AudioClip[] woodenPlankFloor;
    [Space(10)]
    public AudioClip[] sandFloor;
    [Space(10)]
    public AudioClip[] metalCavityFloor;
    [Space(10)]


    #endregion

    #region Attorney

    [Space(20)]
    [Header("Attorney Sounds")]
    [Space(10)]
    public AudioClip[] attorneyVoice;
    [Space(10)]
    public AudioClip[] attorneyTaunting;
    [Space(10)]
    public AudioClip[] attorneyGotHit;
    [Space(10)]
    public AudioClip[] attorneyGenericAttack;
    [Space(10)]
    public AudioClip[] attorneyBlock;
    [Space(10)]
    public AudioClip[] attorneyBlockBreak;
    [Space(10)]
    public AudioClip[] attorneyDeath;
    [Space(10)]
    public AudioClip[] attorneyHeartBeat;
    [Space(10)]
    public AudioClip[] attorneyheavyBreathing;
    [Space(10)]

    #endregion

    #region Teacher
    [Space(20)]
    [Header("Teacher Sounds")]
    [Space(10)]
    public AudioClip[] teacherMaleTaunt;
    [Space(10)]
    public AudioClip[] teacherMaleGotHit;
    [Space(10)]
    public AudioClip[] teacherMaleHit;
    [Space(10)]
    public AudioClip[] teacherMaleDeath;
    [Space(10)]
    public AudioClip[] teacherFemaleTaunt;
    [Space(10)]
    public AudioClip[] teacherFemaleGotHit;
    [Space(10)]
    public AudioClip[] teacherFemaleHit;
    [Space(10)]
    public AudioClip[] teacherFemaleDeath;
    [Space(10)]

    #endregion

    #region Child
    [Space(20)]
    [Header("Children Sounds")]
    [Space(10)]
    public AudioClip[] childHelpVoice;
    [Space(10)]
    public AudioClip[] childCheersVoice;

    #endregion

    // Sound Effect
    public AudioClip Punch()
    {
        return punchSound[Random.Range(0, punchSound.Length)];
    }

    public AudioClip AttackSwoosh()
    {
        return attackSwooshSound[Random.Range(0, attackSwooshSound.Length)];
    }



    public AudioClip Fence()
    {
        return fenceSound[Random.Range(0, fenceSound.Length)];
    }

    public AudioClip FenceStartNotice()
    {
        return fenceStartNoticeSound[Random.Range(0, fenceStartNoticeSound.Length)];
    }

    public AudioClip MetalGateHit()
    {
        return metalGateHitSound[Random.Range(0, metalGateHitSound.Length)];
    }


    public AudioClip ChildNotice(int _number)
    {
        switch (_number)
        {
            case 1:
                return childNoticeSound[0];
            case 2:
                return childNoticeSound[1];
            case 3:
                return childNoticeSound[2];
            case 4:
                return childNoticeSound[3];
            case 5:
                return childNoticeSound[4];
            default:
                return childNoticeSound[0];
        }
    }



    public AudioClip StepSound(GameObject floor)
    {

        SurfaceMaterialSelection _surface = floor.GetComponent<SurfaceMaterialSelection>();

        if (_surface == null)
            return hardFloor[Random.Range(0, hardFloor.Length)];

        switch (_surface.GroundMaterial)
        {
            case SurfaceMaterialSelection.SurfaceMaterial.HardFloor:
                return hardFloor[Random.Range(0, hardFloor.Length)];

            case SurfaceMaterialSelection.SurfaceMaterial.Gras:
                return grasFloor[Random.Range(0, grasFloor.Length)];

            case SurfaceMaterialSelection.SurfaceMaterial.HallWay:
                return hallwayFloor[Random.Range(0, hallwayFloor.Length)];

            case SurfaceMaterialSelection.SurfaceMaterial.WoodenPlank:
                return woodenPlankFloor[Random.Range(0, woodenPlankFloor.Length)];

            case SurfaceMaterialSelection.SurfaceMaterial.Sand:
                return sandFloor[Random.Range(0, sandFloor.Length)];

            case SurfaceMaterialSelection.SurfaceMaterial.MetalCavity:
                return metalCavityFloor[Random.Range(0, metalCavityFloor.Length)];

            default:
                return hardFloor[Random.Range(0, hardFloor.Length)];
        }
    }

    public AudioClip AttorneyheavyBreathing()
    {
        return attorneyheavyBreathing[Random.Range(0, attorneyheavyBreathing.Length)];
    }

    public AudioClip AttorneyVoice()
    {
        return attorneyVoice[Random.Range(0, attorneyVoice.Length)];
    }

    public AudioClip AttorneyTauntingSounds()
    {
        return attorneyTaunting[Random.Range(0, attorneyTaunting.Length)];
    }

    public AudioClip AttorneyGotHit()
    {
        return attorneyGotHit[Random.Range(0, attorneyGotHit.Length)];
    }

    public AudioClip AttorneyGenericAttack()
    {
        return attorneyGenericAttack[Random.Range(0, attorneyGenericAttack.Length)];
    }

    public AudioClip AttorneyBlock()
    {
        return attorneyBlock[Random.Range(0, attorneyBlock.Length)];
    }

    public AudioClip AttorneyBlockBreak()
    {
        return attorneyBlockBreak[Random.Range(0, attorneyBlockBreak.Length)];
    }

    public AudioClip AttorneyDeath()
    {
        return attorneyDeath[Random.Range(0, attorneyDeath.Length)];
    }

    public AudioClip AttorneyHeartBeat()
    {
        return attorneyHeartBeat[Random.Range(0, attorneyHeartBeat.Length)];
    }


    public AudioClip TeacherTaunt(bool _male)
    {

        if (_male)
            return teacherMaleTaunt[Random.Range(0, teacherMaleTaunt.Length)];
        else
            return teacherFemaleTaunt[Random.Range(0, teacherFemaleTaunt.Length)];
    }

    public AudioClip TeacherGotHit(bool _male)
    {
        if (_male)
            return teacherMaleGotHit[Random.Range(0, teacherMaleGotHit.Length)];
        else
            return teacherFemaleGotHit[Random.Range(0, teacherFemaleGotHit.Length)];
    }

    public AudioClip TeacherDeath(bool _male)
    {
        if (_male)
            return teacherMaleDeath[Random.Range(0, teacherMaleDeath.Length)];
        else
            return teacherFemaleDeath[Random.Range(0, teacherFemaleDeath.Length)];
    }

    public AudioClip TeacherAttack(bool _male)
    {
        if (_male)
            return teacherMaleHit[Random.Range(0, teacherMaleDeath.Length)];
        else
            return teacherFemaleHit[Random.Range(0, teacherFemaleDeath.Length)];
    }

    public AudioClip ChildHelpVoice()
    {
        return childHelpVoice[Random.Range(0, childHelpVoice.Length)];
    }

    public AudioClip ChildCheersVoice()
    {
        return childCheersVoice[Random.Range(0, childCheersVoice.Length)];
    }
}