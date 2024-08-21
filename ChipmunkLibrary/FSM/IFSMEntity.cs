using System;
using System.Collections;
using System.Collections.Generic;
using Chipmunk.Library;
using UnityEngine;

namespace Chipmunk.Library
{
    public interface IFSMEntity<TEnumState, TEntity> where TEnumState : Enum where TEntity : IFSMEntity<TEnumState, TEntity>
    {
        public Animator Animator { get; }
        public bool CanChangeState { get; }
        public FSMStateMachine<TEnumState, TEntity> FSMStateMachine { get; }
    }
}