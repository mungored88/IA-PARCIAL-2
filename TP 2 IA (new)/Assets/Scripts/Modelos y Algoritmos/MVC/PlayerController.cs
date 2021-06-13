using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : IController
{
	PlayerModel _model;

    public PlayerController(PlayerModel model, PlayerView view)
	{
		_model = model;

        _model.OnMove += view.Move;
        _model.OnJump += view.Jump;
        _model.OnHealthUpdate += view.HealthUpdate;
        _model.OnGetDamage += view.Hurt;
        _model.OnSpeedUp += view.UpdateSpeedText;
        _model.OnSpeedDown += view.DisableSpeedText;
        _model.OnShieldsChanged += view.UpdateShieldText;
        _model.OnShieldDeactivated += view.ShieldEffectOff;
        _model.OnMagnetChanged += view.UpdateMagnetText;
        _model.OnMagnetDeactivated += view.MagnetEffectOff;
	}

	public void OnUpdate()
	{
		var x = Input.GetAxis("Horizontal");
		var y = Input.GetAxis("Vertical");

		if (x != 0 || y != 0 && !_model.climbing)
			_model.Move(new Vector3(x, 0, y));
		else if (x != 0 || y != 0 && _model.climbing)
			_model.Climb(new Vector3(x, y, 0));

		if (Input.GetKeyDown(KeyCode.Space))
			_model.Jump();
	}
}
