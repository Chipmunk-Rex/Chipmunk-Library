using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chipmunk.Library;
using Unity.Netcode;

namespace Chipmunk.Library.Network
{
    public abstract class NetworkFSMState<TEnumState, TEntity> where TEnumState : Enum where TEntity : INetworkFSMEntity<TEnumState, TEntity>
    {
        protected NetworkFSMStateMachine<TEnumState, TEntity> stateMachine;
        protected TEntity entity {get => stateMachine.entity;}
        protected int animHash;
        /// <summary>
        /// State가 시작될때 호출
        /// </summary>
        public virtual void EnterState()
        {
            if (stateMachine.entity.Animator != null && animHash != 0)
                stateMachine.entity.Animator.SetBool(animHash, true);
        }
        /// <summary>
        /// state를 Update함
        /// </summary>
        public abstract void UpdateState();
        /// <summary>
        /// State가 종료될때 호출
        /// </summary>
        public virtual void ExitState()
        {
            if (stateMachine.entity.Animator != null && animHash != 0)
                stateMachine.entity.Animator.SetBool(animHash, false);
        }
        /// <summary>
        /// targetState에서 이 상태로 바뀔 수 있는지 반환
        /// </summary>
        /// <param name="targetState"></param>
        /// <returns> 바뀔 수 있으면 True, 아니면 False </returns>
        public virtual bool CanChangeTo(TEnumState targetState)
        {
            return true;
        }
        /// <summary>
        /// targetState에서 이 상태로 바뀔 수 있는지 반환
        /// </summary>
        /// <returns> 바뀔 수 있으면 True, 아니면 False </returns>
        public virtual bool CanChangeToThis(TEnumState targetState)
        {
            return true;
        }
        /// <summary>
        /// 현재 State의 애니매이션이 종료되면 호출
        /// </summary>
        public virtual void AnimEnd()
        {

        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            throw new NotImplementedException();
        }

        public NetworkFSMState(INetworkFSMEntity<TEnumState, TEntity> entity, String animName)
        {
            if (animName == "")
                animHash = 0;
            else
                animHash = Animator.StringToHash(animName);
            stateMachine = entity.FSMStateMachine;
        }
    }
}