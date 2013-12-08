using UnityEngine;
using System.Collections;

public class BuffContainer
{
    ArrayList buffs = new ArrayList ();
    
    public BuffContainer ()
    {
        buffs.Clear ();
    }
    
    public bool add (Buff buff)
    {
        if (buff == null) {
            System.Diagnostics.Debug.Assert (false);
        }
        buffs.Add (buff);
        return true;
    }
    
    public bool delete (Buff buff)
    {
        foreach (Buff b in buffs) {
            if (b.Equals (buff)) {
                buffs.Remove (buff);
                return true;
            }
        }
        System.Diagnostics.Debug.Assert (false);
        return false;
    }
    
    public void updateBuffs (float deltaTime)
    {
        foreach (Buff b in buffs) {
            b.checkDuration (deltaTime);
        }
        
        ArrayList arrayToDelete = new ArrayList ();
        foreach (Buff b in buffs) {
            if (b.Duration <= 0.0f) {
                arrayToDelete.Add (b);
            }
        }
        
        foreach (Buff b in arrayToDelete) {
            buffs.Remove (b);
        }
    }
    
    public void applyAllBuffs (RunnerAbility ability)
    {
        foreach (Buff b in buffs) {
            b.apply (ability);  
        }
    }

    public void clear ()
    {
        buffs.Clear();
    }

    public bool isOn(Buff buff)
    {
        foreach (Buff b in buffs) {
            if (b.GetType() == buff.GetType()) {
                return true;
            }
        }
        return false;
    }
}
