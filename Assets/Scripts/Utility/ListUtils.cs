using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    public static class ListUtils
    {
        public static List<T> GetRandomRange<T>(this List<T> list, int count)
        {
            // 0. 리스트 오류 or 개수 오류
            if (list == null || list.Count == 0 || count <= 0)
            {
                return new List<T>();
            }

            // 1. 개수 부족
            if (list.Count < count)
            {
                return list;
            }
            
            // 1. 원본을 망가뜨리지 않기 위해 얕은 복사(사본 리스트) 생성
            List<T> cloneList = new(list);
            List<T> resultList = new();

            // 2. Fisher-Yates Shuffle 원리를 이용해 count만큼만 무작위로 뒤로 밀거나 추출
            for (int i = 0; i < count; i++)
            {
                // 아직 뽑히지 않은 영역(i부터 끝까지)에서 랜덤 인덱스 선택
                int randomIndex = Random.Range(i, cloneList.Count);

                // 스왑(Swap) 연산: 무작위로 뽑힌 녀석을 현재 차례(i)의 녀석과 교환
                (cloneList[i], cloneList[randomIndex]) = (cloneList[randomIndex], cloneList[i]);

                // 스왑되어 앞으로 확정된 기물을 결과 리스트에 담기
                resultList.Add(cloneList[i]);
            }

            return resultList;
        }
    }
}