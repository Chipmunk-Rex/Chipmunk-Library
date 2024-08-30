using System;
using System.Collections;
using System.Collections.Generic;
using Chipmunk.Library;
using Unity.Netcode;
using UnityEngine;

namespace Chipmunk.Library.Network
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEnumState"> state를 정의한 Enum </typeparam>
    /// <typeparam name="TEntity"> FSM을 구현할 스크립트 본인 </typeparam>
    public interface INetworkFSMEntity<TEnumState, TEntity> where TEnumState : Enum where TEntity : IFSMEntity<TEnumState, TEntity>
    {
        /// <summary>
        /// 본인의 Animator 컴포넌트를 반환, null값이어도 상관없음
        /// </summary>
        public Animator Animator { get; }
        /// <summary>
        /// State가 변할 수 있는지 반환
        /// </summary>
        public bool CanChangeState { get; }
        public NetworkFSMStateMachine<TEnumState, TEntity> FSMStateMachine { get; }
    }
}