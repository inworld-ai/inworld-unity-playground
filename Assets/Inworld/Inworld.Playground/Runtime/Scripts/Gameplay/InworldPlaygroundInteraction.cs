/*************************************************************************************************
 * Copyright 2024 Theai, Inc. (DBA Inworld)
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using Inworld.Interactions;
using Inworld.Sample;

namespace Inworld.Playground
{
    public class InworldPlaygroundInteraction : InworldAudioInteraction
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            if (!PlayerController.Instance)
                return;
            PlayerController.Instance.onCanvasOpen.AddListener(OnPause);
            PlayerController.Instance.onCanvasClosed.AddListener(OnPlay);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            if (!PlayerController.Instance)
                return;
            PlayerController.Instance.onCanvasOpen.RemoveListener(OnPause);
            PlayerController.Instance.onCanvasClosed.RemoveListener(OnPlay);
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