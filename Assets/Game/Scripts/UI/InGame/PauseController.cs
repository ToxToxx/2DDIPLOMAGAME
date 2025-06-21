using UnityEngine;
using Zenject;
using InGameInput;
using System;

namespace Game.UI
{
    public class PauseController : IInitializable, ITickable, IDisposable
    {
        private readonly IInputService _input;
        private readonly GameObject _pauseMenu;
        private bool _isPaused;

        public PauseController(IInputService input, GameObject pauseMenu)
        {
            Debug.Log("Initialized");
            _input = input;
            _pauseMenu = pauseMenu;
        }

        public void Initialize()
        {
            _pauseMenu.SetActive(false);
            _isPaused = false;
        }

        public void Tick()
        {
            if (_input.PauseWasPressed)
                TogglePause();
        }

        private void TogglePause()
        {
            _isPaused = !_isPaused;
            _pauseMenu.SetActive(_isPaused);
            Time.timeScale = _isPaused ? 0f : 1f;
        }

        public void Dispose()
        {
            if (_isPaused)
            {
                Time.timeScale = 1f;
                _pauseMenu.SetActive(false);
            }
        }
    }
}
