#pragma strict

var onThisAnim:AnimationClip;
var thisEffect:GameObject;
var here:GameObject;
private var animName:String;
private var time:float;
private var played:boolean=false;

function Start () {


}

function Update () {

animName=onThisAnim.name;


if (GetComponent.<Animation>().IsPlaying(animName)&&played==false)
{
time=0;
played=true;
var effect:GameObject=Instantiate(thisEffect, here.transform.position, here.transform.rotation);
effect.transform.parent=here.transform;

}


if (time<onThisAnim.length)
{
time+=Time.deltaTime;
}
else played=false;





}