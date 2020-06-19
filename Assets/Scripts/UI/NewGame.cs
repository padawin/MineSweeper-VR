using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameType { TYPE_2D, TYPE_3D };
public enum GameDifficulty { Easy, Medium, Hard, Custom };

struct GameSettings {
	public int width;
	public int height;
	public int depth;
	public int mines;

	public GameSettings(int width, int height, int depth, int mines) {
		this.width = width;
		this.height = height;
		this.depth = depth;
		this.mines = mines;
	}
}

public class NewGame : MonoBehaviour
{
	[SerializeField] Button startButton;

	GameType? gameType = null;
	GameDifficulty? gameDifficulty = null;

	[SerializeField] InputField width;
	[SerializeField] InputField height;
	[SerializeField] InputField depth;
	[SerializeField] InputField mines;

	// Settings
	[SerializeField] int Width2DEasy;
	[SerializeField] int Height2DEasy;
	[SerializeField] int Mines2DEasy;
	[SerializeField] int Width2DMedium;
	[SerializeField] int Height2DMedium;
	[SerializeField] int Mines2DMedium;
	[SerializeField] int Width2DHard;
	[SerializeField] int Height2DHard;
	[SerializeField] int Mines2DHard;
	[SerializeField] int DefaultWidth2DCustom = 10;
	[SerializeField] int DefaultHeight2DCustom = 10;
	[SerializeField] int DefaultMines2DCustom = 20;

	[SerializeField] int Width3DEasy;
	[SerializeField] int Height3DEasy;
	[SerializeField] int Depth3DEasy;
	[SerializeField] int Mines3DEasy;
	[SerializeField] int Width3DMedium;
	[SerializeField] int Height3DMedium;
	[SerializeField] int Depth3DMedium;
	[SerializeField] int Mines3DMedium;
	[SerializeField] int Width3DHard;
	[SerializeField] int Height3DHard;
	[SerializeField] int Depth3DHard;
	[SerializeField] int Mines3DHard;
	[SerializeField] int DefaultWidth3DCustom = 10;
	[SerializeField] int DefaultHeight3DCustom = 10;
	[SerializeField] int DefaultDepth3DCustom = 10;
	[SerializeField] int DefaultMines3DCustom = 42;

	// Settings mapping
	Dictionary<GameType?, Dictionary<GameDifficulty?, GameSettings>> settings;

	private void Start() {
		settings = new Dictionary<GameType?, Dictionary<GameDifficulty?, GameSettings>>();
		settings[GameType.TYPE_2D] = new Dictionary<GameDifficulty?, GameSettings>();
		settings[GameType.TYPE_2D][GameDifficulty.Easy] = new GameSettings(Width2DEasy, Height2DEasy, 1, Mines2DEasy);
		settings[GameType.TYPE_2D][GameDifficulty.Medium] = new GameSettings(Width2DMedium, Height2DMedium, 1, Mines2DMedium);
		settings[GameType.TYPE_2D][GameDifficulty.Hard] = new GameSettings(Width2DHard, Height2DHard, 1, Mines2DHard);
		settings[GameType.TYPE_2D][GameDifficulty.Custom] = new GameSettings(DefaultWidth2DCustom, DefaultHeight2DCustom, 1, DefaultMines2DCustom);
		settings[GameType.TYPE_3D] = new Dictionary<GameDifficulty?, GameSettings>();
		settings[GameType.TYPE_3D][GameDifficulty.Easy] = new GameSettings(Width3DEasy, Height3DEasy, Depth3DEasy, Mines3DEasy);
		settings[GameType.TYPE_3D][GameDifficulty.Medium] = new GameSettings(Width3DMedium, Height3DMedium, Depth3DMedium, Mines3DMedium);
		settings[GameType.TYPE_3D][GameDifficulty.Hard] = new GameSettings(Width3DHard, Height3DHard, Depth3DHard, Mines3DHard);
		settings[GameType.TYPE_3D][GameDifficulty.Custom] = new GameSettings(DefaultWidth3DCustom, DefaultHeight3DCustom, DefaultDepth3DCustom, DefaultMines3DCustom);
	}

	public void select2DGameType() {
		selectGameType(GameType.TYPE_2D);
	}

	public void select3DGameType() {
		selectGameType(GameType.TYPE_3D);
	}

	private void selectGameType(GameType gameType) {
		this.gameType = gameType;
		setStartButton();
		displaySettings();
	}

	public void selectEasyGame() {
		selectGameDifficulty(GameDifficulty.Easy);
	}

	public void selectMediumGame() {
		selectGameDifficulty(GameDifficulty.Medium);
	}

	public void selectHardGame() {
		selectGameDifficulty(GameDifficulty.Hard);
	}

	public void selectCustomGame() {
		selectGameDifficulty(GameDifficulty.Custom);
	}

	private void selectGameDifficulty(GameDifficulty gameDifficulty) {
		this.gameDifficulty = gameDifficulty;
		setStartButton();
		displaySettings();
	}

	void setStartButton() {
		startButton.interactable = gameType != null && gameDifficulty != null;
	}

	void displaySettings() {
		if (gameType != null && gameDifficulty != null) {
			width.text = settings[gameType][gameDifficulty].width.ToString();
			height.text = settings[gameType][gameDifficulty].height.ToString();
			depth.text = settings[gameType][gameDifficulty].depth.ToString();
			mines.text = settings[gameType][gameDifficulty].mines.ToString();
		}
	}

	public void startGame() {
		MineSweeperPlayer player = GameObject.FindGameObjectWithTag("Player").GetComponent<MineSweeperPlayer>();
		player.enableGameHands();
		GameObject.FindGameObjectWithTag("grid").GetComponent<MineSweeperGrid>().init(
			settings[gameType][gameDifficulty].width,
			settings[gameType][gameDifficulty].height,
			settings[gameType][gameDifficulty].depth,
			settings[gameType][gameDifficulty].mines
		);
	}
}
