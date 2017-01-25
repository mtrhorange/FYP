#pragma strict

var anims:AnimationClip[];
private var rnd:float;
private var animName:String;
private var time:float;

function Start () {

rnd=Mathf.Round(Random.Range(0, anims.length));
}

function Update () {

animName=anims[rnd].name;
time+=Time.deltaTime;

if (!GetComponent.<Animation>().IsPlaying(animName))
{
time=0;
GetComponent.<Animation>().Play(animName);
}

if (time>anims[rnd].length)
{
rnd=Mathf.Round(Random.Range(0, anims.length));


}



}