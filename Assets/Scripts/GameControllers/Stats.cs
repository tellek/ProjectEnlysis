using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Stats : MonoBehaviour {

	#region All Stats
	public bool PlayerIsDead = false;
	public int ReactorCores;
	[Space]
	[Header ("Base Stats")]
	public int HullCondition = 100;
	public int MaxCondition = 100;
	public int ReactorPower = 100;
	public int MaxPower = 100;
	public int Weapon1Devastation;
	public int Weapon2Devastation;
	[Space]
	[Header ("Attributes Strengths")]
	public int Hull;
	public int Efficiency;
	public int Technology;
	public int Ordnance;
	public int Current;
	public int Fulmination;
	public int Mechanical;
	[Space]
	[Header ("Damage Reduction")]
	public int Impact;
	public int Volt;
	public int Heat;
	public int Radiation;
	public int Nanobots;
	#endregion
	[Space]
	[Header ("UI Elements")]
	public Vector2 location = new Vector2(20, 120);
	public bool lockToBottomOfScreen = true;
	public Vector2 hcSize = new Vector2 (1500, 20);
	public Vector2 rpSize = new Vector2 (1500, 20);
	public Texture2D hcTex;
	public Texture2D rpTex;
	public Texture2D bgTex;
	
	private GUIStyle hcStyle = new GUIStyle();
	private GUIStyle rpStyle = new GUIStyle();
	private GUIStyle bgStyle = new GUIStyle();
	private GUIStyle rcStyle = new GUIStyle();
	private Vector2 rpPos;
	private Vector2 hcPos;
	private Vector2 rcPos;

	void Start () {
		hcStyle.normal.background = hcTex;
		rpStyle.normal.background = rpTex;
		bgStyle.normal.background = bgTex;
		rcStyle.fontSize = 50;
		rcStyle.normal.textColor = Color.white;
	}

	void FixedUpdate () {
		if (HullCondition <= 0) PlayerIsDead = true;
	}

	void Update () {
		//if (PlayerIsDead) //Destroy ship
		if (HullCondition < 0) HullCondition = 0;
		if (HullCondition > MaxCondition) HullCondition = MaxCondition;
		if (ReactorPower < 0) ReactorPower = 0;
		if (ReactorPower > MaxPower) ReactorPower = MaxPower;
	}

	void OnGUI () {
		if (lockToBottomOfScreen){
			rcPos = new Vector2 (location.x, Screen.currentResolution.height - location.y);
			hcPos = new Vector2 (location.x, Screen.currentResolution.height - (location.y - 55));
			rpPos = new Vector2 (location.x, Screen.currentResolution.height - (location.y - 80));
		}
		else{
			rcPos = new Vector2 (location.x, location.y);
			hcPos = new Vector2 (location.x, location.y + 55);
			rpPos = new Vector2 (location.x, location.y + 80);
		}

		// Reactor Cores
		GUI.BeginGroup (new Rect (rcPos.x, rcPos.y, 100, 50));
		GUI.Label(new Rect (0, 0, 100, 50), ReactorCores.ToString() ?? "", rcStyle);
		GUI.EndGroup ();

		// Hull Condition
		GUI.BeginGroup (new Rect (hcPos.x, hcPos.y, MaxCondition * 2, hcSize.y));
		GUI.Box (new Rect (0, 0, hcSize.x, hcSize.y), bgTex, bgStyle);

		GUI.BeginGroup (new Rect (0, 0, HullCondition * 2, hcSize.y));
		GUI.Box (new Rect (0, 0, hcSize.x, hcSize.y), hcTex, hcStyle);
		GUI.EndGroup ();
		GUI.EndGroup ();

		// Reactor Power
		GUI.BeginGroup (new Rect (rpPos.x, rpPos.y, MaxPower * 2, rpSize.y));
		GUI.Box (new Rect (0, 0, rpSize.x, rpSize.y), bgTex, bgStyle);

		GUI.BeginGroup (new Rect (0, 0, ReactorPower * 2, rpSize.y));
		GUI.Box (new Rect (0, 0, rpSize.x, rpSize.y), rpTex, rpStyle);
		GUI.EndGroup ();
		GUI.EndGroup ();
	}

	public void DamageHull (int amount, Resistance? type) {
		switch (type) {
			case Resistance.Impact:
				HullCondition -= CalculateTrueDamage (amount, Impact);
				break;
			case Resistance.Volt:
				HullCondition -= CalculateTrueDamage (amount, Volt);
				break;
			case Resistance.Heat:
				HullCondition -= CalculateTrueDamage (amount, Heat);
				break;
			case Resistance.Radiation:
				HullCondition -= CalculateTrueDamage (amount, Radiation);
				break;
			case Resistance.Nanobots:
				HullCondition -= CalculateTrueDamage (amount, Nanobots);
				break;
			default:
				HullCondition -= amount;
				break;
		}
	}

	public void RepairHull (int amount) {
		if ((MaxCondition - HullCondition) < amount)
			HullCondition = MaxCondition;
		else HullCondition += amount;
	}

	public void UpdateAllStats () {

		int mcAdd = 0;
		int mcCounter = 1;
		while (mcCounter <= Hull) {
			if (mcCounter >= 30 && mcCounter < 60) mcAdd += 3;
			else if (mcCounter >= 60 && mcCounter < 90) mcAdd += 2;
			else if (mcCounter >= 90) mcAdd += 1;
			else mcAdd += 4;
			mcCounter++;
		}
		MaxCondition = 100 + mcAdd;

		int mpAdd = 0;
		int mpCounter = 1;
		while (mpCounter <= Efficiency) {
			if (mpCounter >= 15 && mpCounter < 30) mpAdd += 3;
			else if (mpCounter >= 30 && mpCounter < 45) mpAdd += 2;
			else if (mpCounter >= 45) mpAdd += 1;
			else mpAdd += 4;
			mpCounter++;
		}
		MaxPower = 100 + mpAdd;

		Weapon1Devastation = 0;
		Weapon2Devastation = 0;
	}

	private int CalculateTrueDamage (int amount, int modifier) {
		double hp = Convert.ToDouble (HullCondition);
		double amt = Convert.ToDouble (amount);
		double mod = Convert.ToDouble (modifier);
		double newDamage = mod / 100 * amt;
		return Convert.ToInt32 (amt - newDamage);
	}

}

public enum Resistance { Impact, Volt, Heat, Radiation, Nanobots }
public enum WeaponType { Technology, Ordnance, Current, Fulmination, Mechanical }