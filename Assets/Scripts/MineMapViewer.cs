using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MineMapViewer : MonoBehaviour
{
	[Header("UI References")]
	[SerializeField]
	private ScrollRect _scrollRect;
	[SerializeField]
	private RectTransform _content;
	[SerializeField]
	private GameObject _cellPrefab;

	[Header("Grid Settings")]
	[SerializeField]
	private int _cellSize = 50; // 각 칸 크기 (px)

	[Header("Controller")]
	[SerializeField]
	private MineMapController _controller;

	private int visibleRows;
	private int visibleCols;

	private List<GameObject> cellPool = new List<GameObject>();

	void Start()
	{
		// Content 크기를 전체 데이터 크기에 맞춤
		_content.sizeDelta = new Vector2(MineMapController.Rows * _cellSize, MineMapController.Cols * _cellSize);
		_cellPrefab.GetComponent<RectTransform>().sizeDelta = Vector2.one * _cellSize;

		// Viewport에 보이는 셀 개수 계산 (+1 여유)
		RectTransform viewport = _scrollRect.viewport;
		visibleCols = Mathf.CeilToInt(viewport.rect.width / _cellSize) + 1;
		visibleRows = Mathf.CeilToInt(viewport.rect.height / _cellSize) + 1;

		// 풀링할 셀 생성
		int poolSize = visibleCols * visibleRows;
		for (int i = 0; i < poolSize; i++)
		{
			GameObject cell = Instantiate(_cellPrefab, _content);
			cellPool.Add(cell);
		}

		UpdateCells();
		_scrollRect.onValueChanged.AddListener((_) => UpdateCells());
	}

	void UpdateCells()
	{
		// 스크롤된 위치 계산
		float scrollY = _content.anchoredPosition.y;
		float scrollX = -_content.anchoredPosition.x;

		int firstRow = Mathf.FloorToInt(scrollY / _cellSize);
		int firstCol = Mathf.FloorToInt(scrollX / _cellSize);

		for (int i = 0; i < cellPool.Count; i++)
		{
			int x = i / visibleCols + firstRow;
			int y = i % visibleCols + firstCol;

			GameObject cell = cellPool[i];

			if (x < 0 || x >= MineMapController.Cols || y < 0 || y >= MineMapController.Rows)
			{
				cell.SetActive(false);
				continue;
			}

			cell.SetActive(true);
			RectTransform rt = cell.GetComponent<RectTransform>();
			rt.anchoredPosition = new Vector2(y * _cellSize + _cellSize / 2, - x * _cellSize - _cellSize / 2);
			cell.GetComponentInChildren<TMP_Text>().text = $"{x}, {y}\n{_controller.GetCountResult(x, y)}";
		}
		return ;
	}
}
