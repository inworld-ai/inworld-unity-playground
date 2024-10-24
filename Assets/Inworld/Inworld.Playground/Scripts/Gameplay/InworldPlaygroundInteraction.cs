/*************************************************************************************************
 * Copyright 2024 Theai, Inc. (DBA Inworld)
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using Inworld.Interactions;

namespace Inworld.Playground
{
    public class InworldPlaygroundInteraction : InworldAudioInteraction
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            PlaygroundManager.Instance.OnPause.AddListener(OnPause);
            PlaygroundManager.Instance.OnPlay.AddListener(OnPlay);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            if (!PlaygroundManager.Instance)
                return;
            PlaygroundManager.Instance.OnPause.RemoveListener(OnPause);
            PlaygroundManager.Instance.OnPlay.RemoveListener(OnPlay);
        }

        void OnPlay()
        {
            UnpauseUtterance();
        }

        void OnPause()
        {
            PauseUtterance();
        }
        protected override void UnpauseUtterance()
        {
            base.UnpauseUtterance();
            if (PlaybackSource && PlaybackSource.clip)
                PlaybackSource.UnPause();
        }
        protected override void PauseUtterance()
        {
            m_IsContinueKeyPressed = false;
            if (PlaybackSource && PlaybackSource.clip)
                PlaybackSource.Pause();
        }
    }
}