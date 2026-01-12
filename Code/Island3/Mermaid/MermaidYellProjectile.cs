using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class MermaidYellProjectile
{
    public void Init()
    {
        On.FlyingMermaidLevelYellProjectile.Start += Start;
    }

    protected void Start(On.FlyingMermaidLevelYellProjectile.orig_Start orig, FlyingMermaidLevelYellProjectile self)
    {
        AbstractProj.Start(self);
        movingBackDic[self] = false;
        self.StartCoroutine(move_cr(self));
    }

    private IEnumerator move_cr(FlyingMermaidLevelYellProjectile self)
    {
        self.launchSpeed /= 2;//new
        float speed = self.launchSpeed;
        if (movingBackDic[self])//new start
        {
            speed = 0f;
        }//new end
        float t = 0f;
        while ((!OutOfBounds(self.transform.position) && !movingBackDic[self]) || movingBackDic[self])//new
        //for (;;)
        {
            t += CupheadTime.FixedDelta;
            FlyingMermaidLevelYellProjectile.State state = self.state;
            switch (state)
            {
                case FlyingMermaidLevelYellProjectile.State.Slowing:
                    if (t < self.stopTime)
                    {
                        speed = EaseUtils.EaseOutSine(self.launchSpeed, 0f, t / self.stopTime);
                    }
                    else
                    {
                        speed = 0f;
                        self.state = FlyingMermaidLevelYellProjectile.State.Stopped;
                        t = 0f;
                    }
                    break;
                case FlyingMermaidLevelYellProjectile.State.Stopped:
                    if (t >= self.waitTime)
                    {
                        self.state = FlyingMermaidLevelYellProjectile.State.Tracking;
                        t = 0f;
                        if (self.target == null || self.target.IsDead)
                        {
                            self.target = PlayerManager.GetNext();
                        }
                        if (self.target != null)
                        {
                            self.direction = (self.target.center - self.transform.position).normalized;
                            float angle = MathUtils.DirectionToAngle(self.direction) + 180f;
                            self.transform.SetEulerAngles(new float?(0f), new float?(0f), angle);
                            yield return warning_cr(self, angle);//new
                            self.animator.SetTrigger("Continue");
                        }
                    }
                    break;
                default:
                    if (t < self.attackEaseTime)
                    {
                        speed = EaseUtils.EaseInSine(0f, self.trackSpeed, t / self.attackEaseTime);
                    }
                    else
                    {
                        speed = self.trackSpeed;
                    }
                    break;
            }
            Vector2 pos = self.transform.localPosition;
            pos += speed * CupheadTime.FixedDelta * self.direction;
            self.transform.localPosition = pos;
            yield return new WaitForFixedUpdate();
        }
        movingBackDic[self] = true;
        self.state = FlyingMermaidLevelYellProjectile.State.Stopped;
        self.StartCoroutine(move_cr(self));
        yield break;
    }

    private bool OutOfBounds(Vector3 pos)//new
    {
        return pos.x < -550f || pos.y > 400f || pos.y < -300f;
    }

    private IEnumerator warning_cr(FlyingMermaidLevelYellProjectile self, float angle)//new
    {
        GameObject rectangle = GameObject.CreatePrimitive(PrimitiveType.Cube);
        rectangle.transform.localScale = new Vector3(1500f, 40f, 1f);

        MeshRenderer rectangleRenderer = rectangle.GetComponent<MeshRenderer>();
        rectangleRenderer.material = new Material(Shader.FindObjectOfType<Material>());
        rectangleRenderer.material.color = new Color(255f, 0f, 0f, 0.50f);
        rectangleRenderer.material.SetFloat("_Mode", 3);
        rectangleRenderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        rectangleRenderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        rectangleRenderer.material.SetInt("_ZWrite", 0);
        rectangleRenderer.material.DisableKeyword("_ALPHATEST_ON");
        rectangleRenderer.material.EnableKeyword("_ALPHABLEND_ON");
        rectangleRenderer.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        rectangleRenderer.material.renderQueue = 3000;

        GameObject pivot = new GameObject("Pivot");
        pivot.transform.position = self.transform.position;
        rectangle.transform.parent = pivot.transform;
        rectangle.transform.localPosition = new Vector3(-750f, 0f);
        pivot.transform.SetEulerAngles(null, null, angle);

        yield return CupheadTime.WaitForSeconds(self, 0.5f);
        GameObject.Destroy(rectangle);
        GameObject.Destroy(pivot);
        yield break;
    }

    private Dictionary<FlyingMermaidLevelYellProjectile, bool> movingBackDic = new Dictionary<FlyingMermaidLevelYellProjectile, bool>();
}
