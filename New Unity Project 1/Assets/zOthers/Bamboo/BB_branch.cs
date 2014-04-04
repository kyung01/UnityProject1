#pragma warning disable 0414

using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vector3ExtensionMethods;

class BB_branch
{
    public static BB_branch InitPropagateRandom(Vector3 from,
        Vector2 angleInit, float dis,
        Vector2 angleChange, float disChange,
        int generationMax, float chanceExtend = 100.0f)
    {
        //this functions is going to be a bit long < not cool
        int countGen = 1;
        var b = new BB_branch(from, angleInit, angleChange, dis);
        List<BB_branch> genCurrent = new List<BB_branch>(){b};

        while (countGen++ < generationMax && genCurrent.Count >0)
        {
            List<BB_branch> genNext = new List<BB_branch>();
            foreach (var v in genCurrent)
            {
                for (int i = -1; i <= 1; i += 2)
                    if (Random.Range(0, 100) <= chanceExtend)
                    {
                        //used for testing horiozntal incurring
                        float angleV = angleChange.y + v.angle.y;
                        // used for testing vertical incurring
                        var dirs = new Vector2(3.14f, 3.14f) * .5f - new Vector2(v.angle.y, angleV);
                        dirs = dirs.divide(new Vector2(Mathf.Abs(dirs.x), Mathf.Abs(dirs.y)));
                        int dirVert =(Random.Range(0,5) == 0)?-1:1;
                        //if (angleV > 3.14f || angleV<0 ) continue;
                        genNext.Add(v.extend(new Vector2(i, dirVert), disChange * Random.Range(.7f, 1.0f)));
                    }
            }
            genCurrent = genNext;
        }
        return b;

    }
    // implicit conversion
    public static implicit operator GameObject(BB_branch b)
    {
        var o = new GameObject();
        o.transform.position = b.to;
        return o;  
    }

    Vector3 from, to;
    Vector2 angle,angleChange;
    float dis;
    public List<BB_branch> children = new List<BB_branch>();
    public BB_branch(Vector3 from, Vector2 angleBranch, Vector2 angleChange, float dis)
    {
        angle = angleBranch;
        this.angleChange = angleChange;
        this.from = from;
        this.dis = dis;
        float c = Mathf.Abs( Mathf.Cos(angle.y));
        Vector3 dir = new Vector3(c * Mathf.Cos(angle.x), Mathf.Sin(angle.y), c * Mathf.Sin(angle.x));
        to = from + dir * dis;
    }
    public BB_branch extend(Vector2 angleChangeRatio, float disChangePercentage)
    {
        var angleNew = angleChange.mult(angleChangeRatio);
        var newBranch = new BB_branch(to, angle + angleNew, angleNew, dis * disChangePercentage);
        children.Add(newBranch);
        return newBranch;
    }
}