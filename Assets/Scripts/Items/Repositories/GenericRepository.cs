using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericRepository<T>
{
    protected List<T> m_repoList;

    public GenericRepository()
    {
        m_repoList = new List<T>();
    }

    public IReadOnlyList<T> ReadOnlyList { get { return m_repoList; } }

    public void Clear() => m_repoList.Clear();

    public void Add(T enemy) => m_repoList.Add(enemy);

    public void Remove(T enemy) => m_repoList.Remove(enemy);
}
